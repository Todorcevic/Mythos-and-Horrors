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
        [Inject] protected readonly AudioComponent _audioComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private BoxCollider _collider;
        [SerializeField, Required, AssetsOnly] private AudioClip _clickedAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _hoverOnAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _hoverOffAudio;
        private CardView _cardView;
        private Vector3 _colliderOriginalSize;

        public bool IsClickable { get; set; }
        private CardView CardView => _cardView ??= GetComponentInParent<CardView>(includeInactive: true);

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
        private void OnMouseEnter()
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
            _audioComponent.PlayAudio(_hoverOnAudio);
            CardView.CurrentZoneView.MouseEnter(CardView);
            _cardShowerComponent.ShowCard(CardView);
        }

        private void OnMouseExit()
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
            _audioComponent.PlayAudio(_hoverOffAudio);
            CardView.CurrentZoneView.MouseExit(CardView);
            _cardShowerComponent.HideCard(CardView);
        }

        private void OnMouseUpAsButton()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            MouseUpAsButton();
        }

        public void MouseUpAsButton()
        {
            if (!IsClickable) return;
            _audioComponent.PlayAudio(_clickedAudio);
            _clickHandler.Clicked(CardView);
        }
    }
}
