using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class UndoGameActionIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPlayable
    {
        [SerializeField] private Image _icon;
        [Inject] private readonly ClickHandler<IPlayable> _interactionHandler;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private bool _isPlayable;
        private Effect UndoEffect => _gameActionsProvider.LastInteractable.UndoEffect;
        IEnumerable<Effect> IPlayable.EffectsSelected => new[] { UndoEffect };

        /*******************************************************************/

        private void Start()
        {
            _icon.DOFade(0.5f, 0f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isPlayable) return;
            _interactionHandler.Clicked(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(ViewValues.DEFAULT_SCALE, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        void IPlayable.ActivateToClick()
        {
            _isPlayable = true;
            _icon.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        void IPlayable.DeactivateToClick()
        {
            _isPlayable = false;
            _icon.DOFade(0.5f, ViewValues.FAST_TIME_ANIMATION);
        }
    }
}
