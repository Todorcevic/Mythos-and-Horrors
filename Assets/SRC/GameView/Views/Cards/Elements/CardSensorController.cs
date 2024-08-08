using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CardSensorController : MonoBehaviour
    {
        [Inject] private readonly CardShowerComponent _cardShowerComponent;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [SerializeField, Required, ChildGameObjectsOnly] private BoxCollider _collider;
        private CardView _cardView;
        private Vector3 _colliderOriginalSize;

        public bool IsClickable { get; set; }
        private CardView CardView => _cardView ??= GetComponentInParent<CardView>();

        /*******************************************************************/
        private void Start()
        {
            _colliderOriginalSize = _collider.size;
        }

        /*******************************************************************/
        public void ColliderUp(float amount)
        {
            _collider.size += Vector3.up * amount;
            _collider.center -= 0.5f * amount * Vector3.up;
        }

        public void ColliderDown()
        {
            _collider.size = _colliderOriginalSize;
            _collider.center = Vector3.zero;
        }

        /*******************************************************************/
        public void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            CardView.CurrentZoneView.MouseEnter(CardView);
            _cardShowerComponent.ShowCard(CardView);
        }

        public void OnMouseExit()
        {
            CardView.CurrentZoneView.MouseExit(CardView);
            _cardShowerComponent.HideCard(CardView);
        }

        public void OnMouseUpAsButton()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!IsClickable) return;
            _clickHandler.Clicked(CardView);
        }
    }
}
