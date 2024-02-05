using DG.Tweening;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardSensorController : MonoBehaviour
    {
        private CardView _cardView;
        [Inject] private readonly CardShowerComponent _cardShowerComponent;
        [Inject] private readonly ClickHandler<CardView> _clickHandler;

        public bool IsClickable { get; set; }

        private CardView CardView => _cardView ??= GetComponentInParent<CardView>();

        private Tween tweenToKill;

        /*******************************************************************/
        public void OnMouseEnter()
        {
            tweenToKill?.Kill();
            CardView.CurrentZoneView.MouseEnter(CardView);
            _cardShowerComponent.ShowCard(CardView);
        }

        public void OnMouseExit()
        {
            tweenToKill = CardView.CurrentZoneView.MouseExit(CardView);
            _cardShowerComponent.HideCard(CardView);
        }

        public void OnMouseUpAsButton()
        {
            if (!IsClickable) return;
            _clickHandler.Clicked(CardView);
        }
    }
}
