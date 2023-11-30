using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Diagnostics.CodeAnalysis;
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
        [SerializeField, Required, ChildGameObjectsOnly] private GlowView _glowView;
        [SerializeField, Required, ChildGameObjectsOnly] private CardSensor _cardSensor;
        [SerializeField, Required, ChildGameObjectsOnly] private ZoneCardView _zoneCardView;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rotator;

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
            SetCommonInfo();
            SetAll();
            SetPicture();
        }

        /*******************************************************************/
        public void ActivateToSelect()
        {
            _glowView.SetGreenGlow();
        }

        public Tween MovoToZone(ZoneView zoneView)
        {
            Sequence sequence = DOTween.Sequence();
            if (CurrentZoneView != null) sequence.Join(CurrentZoneView.ExitCard(this));
            return sequence.Join(zoneView.EnterCard(this)).OnStart(() => SetCurrentZoneView(zoneView));
        }

        private void SetCurrentZoneView(ZoneView zoneView)
        {
            CurrentZoneView = zoneView;
            transform.SetParent(zoneView.transform, worldPositionStays: true);
        }

        public void DisableToShow()
        {
            _cardSensor.gameObject.SetActive(false);
            _zoneCardView.gameObject.SetActive(false);
            _glowView.gameObject.SetActive(false);
        }

        public Tween Rotate() => _rotator.DOLocalRotate(new Vector3(0, Card.IsFaceDown ? 180 : 0, 0), ViewValues.FAST_TIME_ANIMATION)
            .SetEase(Ease.InOutExpo);

        public void On()
        {
            gameObject.SetActive(true);
        }

        public void Off()
        {
            gameObject.SetActive(false);
        }

        protected abstract void SetAll();

        private void SetCommonInfo()
        {
            name = Card.Info.Code;
            _title.text = Card.Info.Name;
            _description.text = Card.Info.Description;
            _zoneCardView.Init(Card.OwnZone);
        }

        private void SetPicture()
        {
            _picture.sprite = _picture.sprite; //TODO
        }
    }
}
