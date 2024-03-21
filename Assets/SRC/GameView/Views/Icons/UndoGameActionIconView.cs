using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using UnityEngine.UI;
using System.Linq;

namespace MythosAndHorrors.GameView
{
    public class UndoGameActionIconView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPlayable
    {
        [SerializeField] private Image _icon;
        [Inject] private readonly ClickHandler<IPlayable> _interactionHandler;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private bool _isPlayable;
        private Effect UndoEffect => _gameActionsProvider.GetLastActive<InteractableGameAction>()?.UndoEffect;
        IEnumerable<Effect> IPlayable.EffectsSelected => UndoEffect == null ? Enumerable.Empty<Effect>() : new[] { UndoEffect };

        /*******************************************************************/
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isPlayable) return;
            _interactionHandler.Clicked(this);
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
