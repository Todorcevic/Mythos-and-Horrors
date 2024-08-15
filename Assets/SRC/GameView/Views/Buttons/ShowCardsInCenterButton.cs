using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowCardsInCenterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPlayable
    {
        private bool _isPlayable;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _icon;
        [Inject] private readonly ClickHandler<IPlayable> _interactionHandler;

        IEnumerable<BaseEffect> IPlayable.EffectsSelected => new[] { new BaseEffect(null, null, PlayActionType.None, null, string.Empty, string.Empty) };

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

        public Tween ActivateToClick()
        {
            _isPlayable = true;
            return _icon.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween DeactivateToClick()
        {
            _isPlayable = false;
            return _icon.DOFade(0.5f, ViewValues.FAST_TIME_ANIMATION);
        }

        void IPlayable.ActivateToClick()
        {
            throw new System.NotImplementedException();
        }

        void IPlayable.DeactivateToClick()
        {
            throw new System.NotImplementedException();
        }
    }
}
