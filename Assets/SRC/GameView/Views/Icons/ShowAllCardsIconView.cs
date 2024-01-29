using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowAllCardsIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;

        /*******************************************************************/
        public async void OnPointerClick(PointerEventData eventData)
        {
            await DotweenExtension.WaitForAllTweensToComplete();
            if (_showSelectorComponent.IsVoid) await _showSelectorComponent.ShowPlayables().AsyncWaitForCompletion();
            else if (_showSelectorComponent.IsMultiEffect) await _showSelectorComponent.ReturnClones();
            else await _showSelectorComponent.ReturnPlayables(withActivation: true);
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
