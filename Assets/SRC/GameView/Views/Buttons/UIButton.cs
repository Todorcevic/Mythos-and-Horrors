using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public abstract class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        protected bool _isPlayable;
        [Inject] protected readonly AudioComponent _audioComponent;
        [SerializeField, Required, ChildGameObjectsOnly] protected Image _icon;
        [SerializeField, Required, AssetsOnly] private AudioClip _clickedAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _hoverOnAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _hoverOffAudio;

        /*******************************************************************/
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isPlayable) return;
            _audioComponent.PlayAudio(_clickedAudio);
            Clicked();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isPlayable) return;
            _audioComponent.PlayAudio(_hoverOnAudio);
            transform.DOScale(ViewValues.DEFAULT_SCALE, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isPlayable) return;
            _audioComponent.PlayAudio(_hoverOffAudio);
            transform.DOScale(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        protected abstract void Clicked();
    }
}
