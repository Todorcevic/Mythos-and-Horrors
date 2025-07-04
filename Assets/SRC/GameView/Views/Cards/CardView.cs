using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CardView : MonoBehaviour, IPlayable
    {
        private CardEffect _cloneEffect;
        [Title(nameof(CardView))]
        [SerializeField, Required, ChildGameObjectsOnly] private PictureController _pictureController;
        [SerializeField, Required, ChildGameObjectsOnly] private TitleController _titleController;
        [SerializeField, Required, ChildGameObjectsOnly] private DescriptionController _descriptionController;
        [SerializeField, Required, ChildGameObjectsOnly] private BadgeController _badgeController;
        [SerializeField, Required, ChildGameObjectsOnly] private CostController _costController;
        [SerializeField, Required, ChildGameObjectsOnly] private SkillStatsController _skillStatsController;
        [SerializeField, Required, ChildGameObjectsOnly] private CounterStatsController _countersCollection;
        [SerializeField, Required, ChildGameObjectsOnly] private ChargeController _chargeController;
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _effectController;
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _buffController;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowController _glowComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private CardSensorController _cardSensor;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneRowView _ownZoneCardView;
        [SerializeField, Required, ChildGameObjectsOnly] private RotatorController _rotator;
        [SerializeField, Required, ChildGameObjectsOnly] private CloneComponent _cloneComponent;

        public Card Card { get; private set; }
        public ZoneView CurrentZoneView { get; private set; }
        public CardSensorController CardSensor => _cardSensor;
        public int DeckPosition => Card.CurrentZone.Cards.IndexOf(Card);
        public IEnumerable<BaseEffect> EffectsSelected => _cloneEffect != null ? new[] { _cloneEffect } : Card.PlayableEffects;
        public Sprite Picture => _pictureController.Picture;
        public bool CanBePlayed => EffectsSelected.Any(effect => effect.CanBePlayed);

        /*******************************************************************/
        public void Init(Card card, ZoneView currentZoneView)
        {
            Card = card;
            SetCommon(currentZoneView);
            Off();
        }

        /*******************************************************************/
        private void SetCommon(ZoneView currentZoneView)
        {
            _pictureController.Init(Card);
            _titleController.Init(Card);
            _descriptionController.Init(Card);
            _badgeController.Init(Card);
            _costController.Init(Card);
            _skillStatsController.Init(Card);
            _countersCollection.Init(Card);
            _chargeController.Init(Card);
            HideBuffsAndEffects();

            SetInitialCurrentZoneView(currentZoneView);
            name = Card.Info.Code;
            _ownZoneCardView.Init(Card.OwnZone);

            /*******************************************************************/
            void SetInitialCurrentZoneView(ZoneView zoneView)
            {
                CurrentZoneView = zoneView;
                transform.SetParent(zoneView.transform);
            }
        }

        public Tween DisableToCenterShow()
        {
            if (_ownZoneCardView == null || _ownZoneCardView.IsEmpty) return DOTween.Sequence();
            return _ownZoneCardView.transform.DOScale(0, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween EnableFromCenterShow()
        {
            if (_ownZoneCardView == null || _ownZoneCardView.IsEmpty) return DOTween.Sequence();
            return _ownZoneCardView.transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void On() => gameObject.SetActive(true);

        public void Off() => gameObject.SetActive(false);

        public void RefreshDescription() => _descriptionController.SetDescription(Card);

        public void ActivateToClick()
        {
            if (_cardSensor.IsClickable) return;
            _cardSensor.IsClickable = true;
            _effectController.AddEffects(((IEnumerable<IViewEffect>)((IPlayable)this).EffectsSelected));
            _glowComponent.SetGreenGlow();
        }

        public void DeactivateToClick()
        {
            if (!_cardSensor.IsClickable) return;
            _cardSensor.IsClickable = false;
            _effectController.Clear();
            _glowComponent.Off();
        }

        public Tween CheckExhaust() => Card.Exausted.IsActive ?
            DOTween.Sequence().Join(_pictureController.ExaustAnimation()).Join(_descriptionController.ExaustAnimation()) :
            DOTween.Sequence().Join(_pictureController.UnexaustAnimation()).Join(_descriptionController.UnexaustAnimation());

        public Tween CheckBlancked() => Card.Blancked.IsActive ? _descriptionController.BlankAnimation() : _descriptionController.UnblankAnimation();

        /*******************************************************************/
        public Tween Rotate(bool isInDeck)
        {
            return _rotator.Rotate(Card.FaceDown.IsActive, isInDeck).OnStart(RotateAll);

            /*******************************************************************/
            void RotateAll()
            {
                _effectController.Rotate(Card.FaceDown.IsActive);
                _buffController.Rotate(Card.FaceDown.IsActive);
            }
        }

        public Tween Idle() => transform.DOSpiral(ViewValues.SLOW_TIME_ANIMATION * 4, Vector3.forward, speed: 0.1f, frequency: 1f, depth: 0f, mode: SpiralMode.ExpandThenContract)
                 .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);

        public Tween ShowEffect() => transform.DOLocalMoveY(4, ViewValues.SLOW_TIME_ANIMATION);

        public Tween MoveToZone(ZoneView newZoneView, Ease ease)
        {
            if (newZoneView == CurrentZoneView) return DOTween.Sequence();

            Sequence moveSequence = DOTween.Sequence().SetId(ViewValues.MOVE_ANIMATION)
                 .OnStart(() => transform.SetParent(newZoneView.transform))
                 .Join(CurrentZoneView?.ExitZone(this) ?? DOTween.Sequence())
                 .Join(Rotate(CurrentZoneView is ZoneDeckView))
                 .Join(newZoneView.EnterZone(this).SetEase(ease));

            CurrentZoneView = newZoneView;
            return moveSequence;
        }

        public Sequence RevealAnimation()
        {
            return DOTween.Sequence().Append(DisableToCenterShow())
                             .Append(transform.DOLocalMoveY(8, ViewValues.DEFAULT_TIME_ANIMATION))
                             .Join(_pictureController.UpdateImageAnimation(Card))
                             .Join(_rotator.RotateFake(ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.InCubic))
                             .InsertCallback(ViewValues.DEFAULT_TIME_ANIMATION * 1.5f, ChangeTexts)
                             .Append(transform.DOLocalMoveY(0, ViewValues.DEFAULT_TIME_ANIMATION))
                             .Append(EnableFromCenterShow());

            /*******************************************************************/
            void ChangeTexts()
            {
                _titleController.Init(Card);
                RefreshDescription();
            }
        }

        /*******************************************************************/
        public void SetCloneEffect(CardEffect effect) => _cloneEffect = effect;

        public void AddBuffs()
        {
            if (Card.AffectedByThisBuffs == null || !Card.AffectedByThisBuffs.Any()) return;
            _buffController.AddEffects(Card.AffectedByThisBuffs);
        }

        public void RemoveBuffs() => _buffController.Clear();

        public void ShowBuffsAndEffects()
        {
            _effectController.gameObject.SetActive(true);
            _buffController.gameObject.SetActive(true);
        }

        public void HideBuffsAndEffects()
        {
            _effectController.gameObject.SetActive(false);
            _buffController.gameObject.SetActive(false);
        }

        public int GetBuffsAmount() => _buffController.EffectsAmount;

        /*******************************************************************/
        public CloneComponent CloneToCardShower(Transform parent) => _cloneComponent.Clone(parent);

        public CardView CloneToMultiEffect(Transform parent)
        {
            Transform realParent = _ownZoneCardView.transform.parent;
            _ownZoneCardView.transform.SetParent(transform.parent);
            CardView clone = ZenjectHelper.Instantiate(this, parent);
            _ownZoneCardView.transform.SetParent(realParent);
            clone._ownZoneCardView = null;
            clone.Card = Card;
            return clone;
        }

        public void ColliderForBuffs(float amount) => _cardSensor.ColliderUp(amount);

        public void ColliderRestore() => _cardSensor.ColliderDown();
    }
}
