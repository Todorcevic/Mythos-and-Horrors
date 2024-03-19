using DG.Tweening;
using MythosAndHorrors.GameRules;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class UndoGameActionIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public async void OnPointerClick(PointerEventData eventData)
        {
            await _gameActionsProvider.UndoLastInteractable();
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
