using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardSensorController : MonoBehaviour
    {
        [SerializeField, Required] private CardView _cardView;
        [Inject] private readonly CardShowerComponent _cardShowerComponent;
        [Inject] private readonly ClickHandler<CardView> _clickHandler;

        public bool IsClickable { get; set; }

        /*******************************************************************/
        public void OnMouseEnter()
        {
            DOTween.Kill(_cardView.CurrentZoneView);

            _cardView.CurrentZoneView.MouseEnter(_cardView);
            _cardShowerComponent.ShowCard(_cardView);
        }

        public void OnMouseExit()
        {
            _cardView.CurrentZoneView.MouseExit(_cardView).SetId(_cardView.CurrentZoneView);
            _cardShowerComponent.HideCard(_cardView);
        }

        public void OnMouseUpAsButton()
        {
            if (!IsClickable) return;
            _clickHandler.Clicked(_cardView);
        }
    }
}
