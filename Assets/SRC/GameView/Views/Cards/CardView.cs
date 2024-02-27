using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public abstract class CardView : MonoBehaviour, IPlayable
    {
        [Title("CardView")]
        [SerializeField, Required, ChildGameObjectsOnly] protected TextMeshPro _title;
        [SerializeField, Required, ChildGameObjectsOnly] protected TextMeshPro _description;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _picture;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowController _glowComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private CardSensorController _cardSensor;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneCardView _ownZoneCardView;
        [SerializeField, Required, ChildGameObjectsOnly] protected RotatorController _rotator;
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _effectController;
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _buffController;
        [SerializeField, Required, ChildGameObjectsOnly] private CloneComponent _cloneComponent;

        public bool IsBack => transform.rotation.eulerAngles.y == 180;
        public Card Card { get; private set; }
        public ZoneCardView OwnZone => _ownZoneCardView;
        public RotatorController Rotator => _rotator;
        public ZoneView CurrentZoneView { get; private set; }
        public int DeckPosition => Card.CurrentZone.Cards.IndexOf(Card);

        /*******************************************************************/
        public void Init(Card card)
        {
            Card = card;
            SetPicture();
            SetCommon();
            SetSpecific();
        }

        public void InitClone(Card card)
        {
            Card = card;
        }

        /*******************************************************************/
        public void SetCurrentZoneView(ZoneView zoneView)
        {
            CurrentZoneView = zoneView;
            transform.SetParent(zoneView.transform);
        }

        public Tween DisableToCenterShow()
        {
            if (_ownZoneCardView.IsEmpty) return DOTween.Sequence();
            return _ownZoneCardView.transform.DOScale(0, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween EnableFromCenterShow()
        {
            if (_ownZoneCardView.IsEmpty) return DOTween.Sequence();
            return _ownZoneCardView.transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween Rotate() => _rotator.Rotate(Card.FaceDown.IsActive);

        public void On() => gameObject.SetActive(true);

        public void Off() => gameObject.SetActive(false);

        public void ActivateToClick()
        {
            if (_cardSensor.IsClickable) return;
            _glowComponent.SetGreenGlow();
            _cardSensor.IsClickable = true;
        }

        public void DeactivateToClick()
        {
            if (!_cardSensor.IsClickable) return;
            _glowComponent.Off();
            _cardSensor.IsClickable = false;
        }

        protected abstract void SetSpecific();

        private void SetCommon()
        {
            name = Card.Info.Code;
            _ownZoneCardView.Init(Card.OwnZone);
            _title.text = Card.Info.Name;
            SetDescription(Card.Info.Description ?? Card.Info.Flavor);
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

        private async void SetPicture() => await _picture.LoadCardSprite(Card.Info.Code);

        /*******************************************************************/
        public Tween Idle() => transform.DOSpiral(ViewValues.SLOW_TIME_ANIMATION, Vector3.up, speed: 1f, frequency: 5, depth: 0, mode: SpiralMode.ExpandThenContract)
                 .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetId("Idle");

        public Tween MoveToZone(ZoneView newZoneView, Ease ease = Ease.InOutCubic)
        {
            Sequence moveSequence = DOTween.Sequence()
                 .OnStart(() => transform.SetParent(newZoneView.transform))
                 .Append(CurrentZoneView?.ExitZone(this) ?? DOTween.Sequence())
                 .Join(Rotate())
                 .Join(newZoneView.EnterZone(this).SetEase(ease));

            CurrentZoneView = newZoneView;
            return moveSequence;
        }

        public virtual Sequence RevealAnimation() => DOTween.Sequence().Append(DOTween.Sequence()
                 .Append(DisableToCenterShow())
                 .Append(transform.DOLocalMoveY(8, ViewValues.DEFAULT_TIME_ANIMATION))
                 .Join(_rotator.RotateFake(ViewValues.DEFAULT_TIME_ANIMATION).SetEase(Ease.InCubic))
                 .Append(transform.DOLocalMoveY(0, ViewValues.DEFAULT_TIME_ANIMATION))
                 .Append(EnableFromCenterShow()));

        /*******************************************************************/
        public Effect CloneEffect { get; private set; }
        public void SetCloneEffect(Effect effect) => CloneEffect = effect;
        public void ClearCloneEffect() => CloneEffect = null;

        public void ShowBuffsAndEffects()
        {
            if (CloneEffect != null) _effectController.AddEffects(CloneEffect);
            else _effectController.AddEffects(Card.PlayableEffects.ToArray());

            _buffController.AddEffects(Card.Buffs.ToArray());
        }

        public void HideBuffsAndEffects()
        {
            _effectController.Clear();
            _buffController.Clear();
        }

        public int GetBuffsAmount() => _buffController.EffectsAmount;

        /*******************************************************************/
        public CloneComponent Clone(Transform parent) => _cloneComponent.Clone(parent);

        public void ColliderForBuffs(float amount) => _cardSensor.ColliderUp(amount);

        public void ColliderRestore() => _cardSensor.ColliderDown();
    }
}
