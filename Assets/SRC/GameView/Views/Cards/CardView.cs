using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public abstract class CardView : MonoBehaviour
    {
        [Title("CardView")]
        [SerializeField, Required, ChildGameObjectsOnly] protected TextMeshPro _title;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _description;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _picture;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowController _glowComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private CardSensorController _cardSensor;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneCardView _ownZoneCardView;
        [SerializeField, Required, ChildGameObjectsOnly] private RotatorController _rotator;
        [SerializeField, Required, ChildGameObjectsOnly] private EffectController _effectController;
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

        public void DisableToCenterShow()
        {
            _ownZoneCardView.gameObject.SetActive(false);
        }

        public void EnableToCenterShow()
        {
            _ownZoneCardView.gameObject.SetActive(true);
        }

        public Tween Rotate() => _rotator.Rotate(Card.IsFaceDown);

        public void On() => gameObject.SetActive(true);

        public void Off() => gameObject.SetActive(false);

        public void ActivateToClick()
        {
            _glowComponent.SetGreenGlow();
            _cardSensor.IsClickable = true;
        }

        public void DeactivateToClick()
        {
            _glowComponent.Off();
            _cardSensor.IsClickable = false;
        }

        protected abstract void SetSpecific();

        private void SetCommon()
        {
            name = Card.Info.Code;
            _title.text = Card.Info.Name;
            SetDescription();
            _ownZoneCardView.Init(Card.OwnZone);

            void SetDescription()
            {
                _description.text = "";
                if (Card.Info.Tags != null && Card.Info.Tags.Length > 0)
                {
                    _description.text = "<size=3><b>";
                    Card.Info.Tags.ForEach(tag => _description.text += tag + ", ");
                    _description.text = _description.text.Remove(_description.text.Length - 2);
                    _description.text += "</b></size>\n";
                }

                _description.text += "\n<voffset=0.5em>" + Card.Info.Description + "</voffset>" ?? Card.Info.Flavor;
            }
        }

        private async void SetPicture() => await _picture.LoadCardSprite(Card.Info.Code);

        /*******************************************************************/
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

        public void ShowEffects()
        {
            if (!Card.CanPlay) return;
            _effectController.AddEffects(Card.PlayableEffects.ToArray());
        }

        public void ShowEffect(Effect effect)
        {
            if (!Card.CanPlay) return;
            _effectController.AddEffects(effect);
        }

        public void HideEffects() => _effectController.Clear();

        public CloneComponent Clone(Transform parent) => _cloneComponent.Clone(parent);
    }
}
