using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardSensor : MonoBehaviour
    {
        [SerializeField, Required] private CardView _cardView;
        [Inject] private readonly CardShowerComponent _cardShowerComponent;

        /*******************************************************************/
        public void OnMouseEnter()
        {
            DOTween.KillAll();
            _cardShowerComponent.ShowCard(_cardView);
            _cardView.CurrentZoneView.MouseEnter(_cardView);
        }

        public void OnMouseExit()
        {
            _cardShowerComponent.HideCard(_cardView);
            _cardView.CurrentZoneView.MouseExit(_cardView);
        }

        public void OnMouseDrag()
        {

        }
    }
}
