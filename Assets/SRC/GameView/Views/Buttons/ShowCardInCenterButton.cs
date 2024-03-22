using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowCardInCenterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private bool _isPlayable;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _icon;
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly ClickHandler<IPlayable> _interactionHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;

        /*******************************************************************/
        public async void OnPointerClick(PointerEventData eventData)
        {
            if (!_isPlayable) return;
            if (_showSelectorComponent.IsMultiEffect) _interactionHandler.Clicked(_mainButtonComponent);
            else if (_showSelectorComponent.IsShowing) await _showSelectorComponent.ReturnPlayableWithActivation();
            else await _showSelectorComponent.ShowPlayables();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isPlayable) return;
            transform.DOScale(ViewValues.DEFAULT_SCALE, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isPlayable) return;
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween ActivateToClick()
        {
            _isPlayable = true;
            return _icon.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween DeactivateToClick()
        {
            _isPlayable = false;
            return _icon.DOFade(0.5f, ViewValues.FAST_TIME_ANIMATION);
        }
    }
}
