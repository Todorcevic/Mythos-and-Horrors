using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardSensor : MonoBehaviour
    {
        private const string MOUSE_EXIT = "mouse_exit";
        [SerializeField, Required] private CardView _cardView;
        [Inject] private readonly CardShowerComponent _cardShowerComponent;

        /*******************************************************************/
        public void OnMouseEnter()
        {
            DOTween.Kill(MOUSE_EXIT);
            _cardShowerComponent.ShowCard(_cardView);
            _cardView.CurrentZoneView.MouseEnter(_cardView);
        }

        public void OnMouseExit()
        {
            _cardShowerComponent.HideCard(_cardView);
            _cardView.CurrentZoneView.MouseExit(_cardView).SetId(MOUSE_EXIT);
        }

        public void OnMouseDrag()
        {

        }
    }
}
