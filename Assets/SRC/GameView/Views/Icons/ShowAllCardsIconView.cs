using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowAllCardsIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;

        /*******************************************************************/
        public async void OnPointerClick(PointerEventData eventData)
        {
            _ioActivatorComponent.DeactivateCardSensors();
            await DotweenExtension.WaitForAllTweensToComplete();
            if (_showSelectorComponent.IsMultiEffect) await _showSelectorComponent.ReturnClones();
            else if (!_showSelectorComponent.IsShowing) await _showSelectorComponent.ShowPlayables().AsyncWaitForCompletion();
            else await _showSelectorComponent.ReturnPlayables();
            _ioActivatorComponent.ActivateCardSensors();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(ViewValues.DEFAULT_SCALE, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
        }
    }
}
