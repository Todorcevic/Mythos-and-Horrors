using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ButtonAsGroup : MonoBehaviour
    {
        [SerializeField, Required] private CardSensorController cardSensorController;
        public event Action OnClick;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnMouseEnter()
        {
            cardSensorController.OnMouseExitAnimation.Kill();
            transform.DOScale(1.2f, ViewValues.FAST_TIME_ANIMATION);
        }

        private void OnMouseExit()
        {
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseUpAsButton() => OnClick?.Invoke();
    }
}
