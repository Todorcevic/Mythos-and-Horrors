using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using UnityEngine.UI;
using System.Linq;
using Sirenix.OdinInspector;

namespace MythosAndHorrors.GameView
{
    public class UndoGameActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPlayable
    {
        private bool _isPlayable;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _icon;
        [Inject] private readonly ClickHandler<IPlayable> _interactionHandler;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private BaseEffect UndoEffect => _gameActionsProvider.CurrentInteractable?.UndoEffect;
        IEnumerable<BaseEffect> IPlayable.EffectsSelected => UndoEffect == null ? Enumerable.Empty<Effect>() : new[] { UndoEffect };

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
