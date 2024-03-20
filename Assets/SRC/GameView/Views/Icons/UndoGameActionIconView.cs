using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MythosAndHorrors.GameView
{
    public class UndoGameActionIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        /*******************************************************************/
        public void OnPointerClick(PointerEventData eventData)
        {

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
