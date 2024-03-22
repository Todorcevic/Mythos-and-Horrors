using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowAllCardsIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private bool _isPlayable;
        [SerializeField] private Image _icon;
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

        public void ActivateToClick()
        {
            _isPlayable = true;
            _icon.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void DeactivateToClick()
        {
            _isPlayable = false;
            _icon.DOFade(0.5f, ViewValues.FAST_TIME_ANIMATION);
        }
    }
}
