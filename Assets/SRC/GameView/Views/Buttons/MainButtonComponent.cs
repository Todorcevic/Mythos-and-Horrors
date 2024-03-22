using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class MainButtonComponent : MonoBehaviour, IPlayable
    {
        private const float OFFSET = 1f;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private MeshRenderer _buttonRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _message;
        [SerializeField, Required, ChildGameObjectsOnly] private BoxCollider _collider;
        [SerializeField, Required] private Color _activateColor;
        [SerializeField, Required] private Color _deactivateColor;

        private bool IsActivated => _collider.enabled;
        private Effect MainButtonEffect => _gameActionsProvider.GetRealLastActive<InteractableGameAction>()?.MainButtonEffect;
        IEnumerable<Effect> IPlayable.EffectsSelected => MainButtonEffect == null ? Enumerable.Empty<Effect>() : new[] { MainButtonEffect };

        /*******************************************************************/
        public void ActivateToClick()
        {
            if (IsActivated) return;
            _message.text = MainButtonEffect?.Description;
            _collider.enabled = true;
            _buttonRenderer.transform.DOScaleZ(1f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear);
            _buttonRenderer.material.DOColor(_activateColor, ViewValues.FAST_TIME_ANIMATION);
            _message.transform.DOScale(Vector3.one * 0.005f, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InOutBack, 3f);
            _message.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void DeactivateToClick()
        {
            if (!IsActivated) return;
            _collider.enabled = false;
            _buttonRenderer.transform.DOScaleZ(0.75f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear);
            _buttonRenderer.material.DOColor(_deactivateColor, ViewValues.FAST_TIME_ANIMATION);
            _message.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION);
            _message.DOFade(0f, ViewValues.FAST_TIME_ANIMATION);
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION);
        }

        public Tween MoveToShowSelector(Transform scenePoint)
        {
            if (!((IPlayable)this).CanBePlayed) return DOTween.Sequence();
            return DOTween.Sequence()
                     .Join(transform.DOMove(ButtonPositionInUI(), ViewValues.DEFAULT_TIME_ANIMATION))
                     .Join(transform.DOScale(scenePoint.lossyScale, ViewValues.DEFAULT_TIME_ANIMATION))
                     .SetEase(Ease.InOutSine);

            Vector3 ButtonPositionInUI()
            {
                float distanceBorderRight = (OFFSET - (Screen.width - Camera.main.WorldToScreenPoint(scenePoint.position).x) / Screen.width);
                return new Vector3(scenePoint.position.x - distanceBorderRight, scenePoint.position.y, scenePoint.position.z);
            }
        }

        public Tween RestorePosition() => DOTween.Sequence()
            .Join(transform.DOLocalMove(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
            .Join(transform.DOScale(Vector3.one, ViewValues.DEFAULT_TIME_ANIMATION))
            .Join(transform.DOLocalRotate(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
            .SetEase(Ease.InOutSine);

        /*******************************************************************/
        public void OnMouseEnter() => _light.DOIntensity(2f, ViewValues.FAST_TIME_ANIMATION);

        public void OnMouseExit() => _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION);

        public void OnMouseUpAsButton() => _clickHandler.Clicked(this);
    }
}
