using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView.NEWS
{
    public abstract class CardView : MonoBehaviour, IPlayable
    {
        private CardEffect _cloneEffect;
        [Title(nameof(CardView))]
        [SerializeField, Required, ChildGameObjectsOnly] protected TextMeshPro _title;
        [SerializeField, Required, ChildGameObjectsOnly] protected TextMeshPro _description;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _picture;
        [SerializeField, Required, ChildGameObjectsOnly] private BadgeController _badgeController;
        [SerializeField, Required, ChildGameObjectsOnly] private SkillStatsController _skillStstController;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowController _glowComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private CardSensorController _cardSensor;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneCardView _ownZoneCardView;
        [SerializeField, Required, ChildGameObjectsOnly] private RotatorController _rotator;
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _effectController;
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _buffController;
        [SerializeField, Required, ChildGameObjectsOnly] private CloneComponent _cloneComponent;
        [Inject] private readonly DiContainer _diContainer;

        public Card Card { get; private set; }
        public ZoneView CurrentZoneView { get; private set; }
        public CardSensorController CardSensor => _cardSensor;
        public int DeckPosition => Card.CurrentZone.Cards.IndexOf(Card);
        public IEnumerable<BaseEffect> EffectsSelected => _cloneEffect != null ? new[] { _cloneEffect } : Card.PlayableEffects;

        /*******************************************************************/
        public void Init(Card card, ZoneView currentZoneView)
        {
            Card = card;
            SetCommon(currentZoneView);
            SetSpecific();
            Off();
        }

        /*******************************************************************/
        private void SetCommon(ZoneView currentZoneView)
        {
            SetPicture();
            SetInitialCurrentZoneView(currentZoneView);
            name = Card.Info.Code;
            _ownZoneCardView.Init(Card.OwnZone);
            _title.text = Card.Info.Name;
            SetDescription(Card.Info.Description ?? Card.Info.Flavor);



            _badgeController.SetBadge(Card.Info.Faction);
            _skillStstController.SetStats(Card);

            /*******************************************************************/
            async void SetPicture() => await _picture.LoadCardSprite(Card.Info.Code);

            void SetInitialCurrentZoneView(ZoneView zoneView)
            {
                CurrentZoneView = zoneView;
                transform.SetParent(zoneView.transform);
            }
        }

        protected void SetDescription(string description)
        {
            _description.text = "";
            if (Card.Info.Tags != null && Card.Info.Tags.Length > 0)
            {
                _description.text = "<size=3><b>";
                Card.Info.Tags.ForEach(tag => _description.text += tag + " - ");
                _description.text = _description.text.Remove(_description.text.Length - 3);
                _description.text += "</b></size>\n";
            }

            _description.text += "\n<voffset=0.25em>" + description + "</voffset>";
        }

        protected abstract void SetSpecific();

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


        public void ActivateToClick()
        {
            if (_cardSensor.IsClickable) return;
            _cardSensor.IsClickable = true;
            _glowComponent.SetGreenGlow();
            _effectController.AddEffects(((IEnumerable<IViewEffect>)((IPlayable)this).EffectsSelected));
        }

        public void DeactivateToClick()
        {
            if (!_cardSensor.IsClickable) return;
            _cardSensor.IsClickable = false;
            _glowComponent.Off();
            _effectController.Clear();
        }

        public Tween CheckExhaust() =>
            _picture.material.DOColor(Card.Exausted.IsActive ? ViewValues.DEACTIVE_COLOR : Color.white, ViewValues.DEFAULT_TIME_ANIMATION);

        public Tween CheckBlancked()
        {
            SetDescription(Card.Blancked.IsActive ? string.Empty : Card.Info.Description ?? Card.Info.Flavor);
            return DOTween.Sequence();
        }

        /*******************************************************************/
        public Tween Rotate()
        {
            _effectController.Rotate(Card.FaceDown.IsActive);
            _buffController.Rotate(Card.FaceDown.IsActive);
            return _rotator.Rotate(Card.FaceDown.IsActive);
        }

        public Tween Idle() => transform.DOSpiral(ViewValues.SLOW_TIME_ANIMATION, Vector3.up, speed: 1f, frequency: 5, depth: 0, mode: SpiralMode.ExpandThenContract)
                 .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetId("Idle");

        //public Tween MoveToZone(ZoneView newZoneView, Ease ease = Ease.InOutCubic)
        //{
        //    if (newZoneView == CurrentZoneView) return DOTween.Sequence();
        //    Sequence moveSequence = DOTween.Sequence().SetId(nameof(CardView))
        //         .OnStart(() => transform.SetParent(newZoneView.transform))
        //         .Append(CurrentZoneView?.ExitZone(this) ?? DOTween.Sequence())
        //         .Join(Rotate())
        //         .Join(newZoneView.EnterZone(this).SetEase(ease));

        //    CurrentZoneView = newZoneView;
        //    return moveSequence;
        //}

        public virtual Sequence RevealAnimation() => DOTween.Sequence()
                            .Append(DisableToCenterShow())
                            .Append(transform.DOLocalMoveY(8, ViewValues.DEFAULT_TIME_ANIMATION))
                            .Join(_rotator.RotateFake(ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.InCubic))
                            .Append(transform.DOLocalMoveY(0, ViewValues.DEFAULT_TIME_ANIMATION))
                            .Append(EnableFromCenterShow());

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
            CardView clone = _diContainer.InstantiatePrefabForComponent<CardView>(this, parent);
            _ownZoneCardView.transform.SetParent(realParent);
            clone._ownZoneCardView = null;
            clone.Card = Card;
            return clone;
        }

        public void ColliderForBuffs(float amount) => _cardSensor.ColliderUp(amount);

        public void ColliderRestore() => _cardSensor.ColliderDown();
    }
}
