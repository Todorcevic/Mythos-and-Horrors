﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowAllCardsIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Inject] private readonly ShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly ClickHandler<IPlayable> _interactionHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;

        /*******************************************************************/
        public async void OnPointerClick(PointerEventData eventData)
        {
            if (_showSelectorComponent.IsMultiEffect) _interactionHandler.Clicked(_mainButtonComponent);
            else if (!_showSelectorComponent.IsShowing) await _showSelectorComponent.ShowPlayables();
            else await _showSelectorComponent.ReturnPlayableWithActivation();
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