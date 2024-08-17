using MythosAndHorrors.GameRules;
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
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [SerializeField, Required, ChildGameObjectsOnly] private BoxCollider _collider;
        private CardView _cardView;
        private Vector3 _colliderOriginalSize;

        public bool IsClickable { get; set; }

        /*******************************************************************/
        public void Init(CardView cardView)
        {
            _cardView = cardView;
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
            MouseEnter();
            CheckOnMouseEnterAvatar();

            void CheckOnMouseEnterAvatar()
            {
                if (_cardView.Card is not CardAvatar cardAvatar) return;
                _avatarViewsManager.Get(cardAvatar.Owner).PointerEnterAnimation();
            }
        }

        public void MouseEnter()
        {
            _cardView.CurrentZoneView.MouseEnter(_cardView);
            _cardShowerComponent.ShowCard(_cardView);
        }

        public void OnMouseExit()
        {
            MouseExit();
            CheckOnMouseExitAvatar();

            void CheckOnMouseExitAvatar()
            {
                if (_cardView.Card is not CardAvatar cardAvatar) return;
                _avatarViewsManager.Get(cardAvatar.Owner).PointerExitAnimation();
            }
        }

        public void MouseExit()
        {
            _cardView.CurrentZoneView.MouseExit(_cardView);
            _cardShowerComponent.HideCard(_cardView);
        }

        public void OnMouseUpAsButton()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!IsClickable) return;
            _clickHandler.Clicked(_cardView);
        }
    }
}
