using DG.Tweening;
using System;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ButtonAsGroup : MonoBehaviour
    {
        public event Action OnClick;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnMouseEnter() => transform.DOScale(1.1f, ViewValues.FAST_TIME_ANIMATION);

        private void OnMouseExit() => transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);

        private void OnMouseUpAsButton() => OnClick?.Invoke();
    }
}
