﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class MainButtonComponent : MonoBehaviour, IPlayable
    {
        private const float OFFSET = 1f;
        [Inject] private readonly ClickHandler _clickHandler;
        [Inject] private readonly TextsManager _textsProvider;
        [Inject] private readonly AudioComponent _audioComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private MeshRenderer _buttonRenderer;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _message;
        [SerializeField, Required, ChildGameObjectsOnly] private BoxCollider _collider;
        [SerializeField, Required] private Color _activateColor;
        [SerializeField, Required] private Color _deactivateColor;
        [SerializeField, Required, AssetsOnly] private AudioClip _clickedAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _hoverOnAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _hoverOffAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _glowOnAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _glowOffAudio;

        public BaseEffect MainButtonEffect { get; private set; }
        IEnumerable<BaseEffect> IPlayable.EffectsSelected => MainButtonEffect == null ? Enumerable.Empty<CardEffect>() : new[] { MainButtonEffect };
        public bool IsActivated => _collider.enabled;
        public bool CanBePlayed => MainButtonEffect != null;

        /*******************************************************************/
        public void SetEffect(BaseEffect effect) => MainButtonEffect = effect;

        public void ActivateToClick()
        {
            _message.text = _textsProvider.GetLocalizableText(MainButtonEffect.Localization);
            DOTween.Sequence().OnPlay(() => _audioComponent.PlayAudio(_glowOnAudio))
                .Join(_buttonRenderer.transform.DOScaleZ(1f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear))
                .Join(_buttonRenderer.material.DOColor(_activateColor, ViewValues.FAST_TIME_ANIMATION))
                .Join(_message.transform.DOScale(Vector3.one * 0.005f, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InOutBack, 3f))
                .Join(_message.DOFade(1f, ViewValues.FAST_TIME_ANIMATION)).OnComplete(() => _collider.enabled = true);
        }

        public void DeactivateToClick()
        {
            _collider.enabled = false;
            DOTween.Sequence().OnPlay(() => _audioComponent.PlayAudio(_glowOffAudio))
                 .Join(_buttonRenderer.transform.DOScaleZ(0.75f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear))
                .Join(_buttonRenderer.material.DOColor(_deactivateColor, ViewValues.FAST_TIME_ANIMATION))
                .Join(_message.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION))
                .Join(_message.DOFade(0f, ViewValues.FAST_TIME_ANIMATION))
                .Join(_light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION));
        }

        public void ActivateToCancelClick()
        {
            _message.text = _textsProvider.GetLocalizableText(MainButtonEffect.Localization);
            _collider.enabled = true;
            _buttonRenderer.transform.DOScaleZ(1f, ViewValues.FAST_TIME_ANIMATION * 0.5f).SetEase(Ease.Linear);
            _buttonRenderer.material.DOColor(_deactivateColor, ViewValues.FAST_TIME_ANIMATION);
            _message.transform.DOScale(Vector3.one * 0.005f, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InOutBack, 3f);
            _message.DOFade(1f, ViewValues.FAST_TIME_ANIMATION);
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
        public void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            _audioComponent.PlayAudio(_hoverOnAudio);
            _light.DOIntensity(2f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseExit()
        {
            _audioComponent.PlayAudio(_hoverOffAudio);
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseUpAsButton()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            _audioComponent.PlayAudio(_clickedAudio);
            _clickHandler.Clicked(this);
        }
    }
}
