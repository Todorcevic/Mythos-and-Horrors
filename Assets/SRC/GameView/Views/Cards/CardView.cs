using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public abstract class CardView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _title;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _description;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _picture;
        [SerializeField, Required, ChildGameObjectsOnly] private GlowComponent _glowComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private CardSensorComponent _cardSensor;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneCardView _zoneCardView;
        [SerializeField, Required, ChildGameObjectsOnly] private RotatorComponent _rotator;

        public bool IsBack => transform.rotation.eulerAngles.y == 180;
        public Card Card { get; private set; }
        public ZoneCardView OwnZone => _zoneCardView;
        public ZoneView CurrentZoneView { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init(Card card)
        {
            Card = card;
            SetPicture();
            SetCommon();
            SetSpecific();
        }

        /*******************************************************************/
        public void SetCurrentZoneView(ZoneView zoneView)
        {
            CurrentZoneView = zoneView;
            transform.SetParent(zoneView.transform);
        }

        public void DisableToShow()
        {
            _cardSensor.gameObject.SetActive(false);
            _zoneCardView.gameObject.SetActive(false);
            _glowComponent.gameObject.SetActive(false);
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
            _description.text = Card.Info.Description ?? Card.Info.Flavor;
            _zoneCardView.Init(Card.OwnZone);
        }

        private async void SetPicture() => await _picture.LoadCardSprite(Card.Info.Code);

        public Tween MoveToZone(ZoneView newZoneView, Ease ease = Ease.InOutCubic)
        {
            Sequence moveSequence = DOTween.Sequence()
                .Join(CurrentZoneView.ExitZone(this))
                .Join(Rotate())
                .Join(newZoneView.EnterZone(this).SetEase(ease));

            SetCurrentZoneView(newZoneView);
            return moveSequence;
        }
    }
}
