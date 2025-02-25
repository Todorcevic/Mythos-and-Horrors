using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class TokensPileComponent : MonoBehaviour, IStatable, IPlayable
    {
        private bool _isClickable;
        private const float MOVE_OFFSET = 8;
        private const float Y_OFF_SET = 1f;
        private const float Z_OFF_SET = 2f;
        private const float LIGHT_INTENSITY = 8f;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly ClickHandler _clickHandler;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly AudioComponent _audioComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _showToken;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;
        [SerializeField, Required, AssetsOnly] private AudioClip _glowOnAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _glowOffAudio;
        [SerializeField, Required, AssetsOnly] private AudioClip _hoverOnToken;
        [SerializeField, Required, AssetsOnly] private AudioClip _hoverOffToken;

        public CardEffect TakeResourceEffect => (_gameActionsProvider.CurrentInteractable as InvestigatorTurnGameAction)?.TakeResourceEffect;
        IEnumerable<BaseEffect> IPlayable.EffectsSelected => TakeResourceEffect == null ? Enumerable.Empty<CardEffect>() : new[] { TakeResourceEffect };
        public bool CanBePlayed => TakeResourceEffect?.CanBePlayed ?? false;

        /*******************************************************************/
        Transform IStatable.StatTransform => _showToken;
        Stat IStatable.Stat => _chaptersProvider.CurrentScene.PileAmount;
        Tween IStatable.UpdateAnimation() => DOTween.Sequence();

        /*******************************************************************/
        public void ActivateToClick()
        {
            if (_isClickable) return;
            _light.DOIntensity(LIGHT_INTENSITY, ViewValues.FAST_TIME_ANIMATION)
                .OnPlay(() => _audioComponent.PlayAudio(_glowOnAudio))
                .OnComplete(() => _isClickable = true);
        }

        public void DeactivateToClick()
        {
            if (!_isClickable) return;
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION)
                .OnPlay(() => _audioComponent.PlayAudio(_glowOffAudio))
                .OnComplete(() => _isClickable = false);
        }

        /*******************************************************************/
        public void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            DOTween.Sequence().OnStart(() => _audioComponent.PlayAudio(_hoverOnToken))
                .Join(_showToken.DOLocalMoveY(Y_OFF_SET, ViewValues.FAST_TIME_ANIMATION))
                .Join(_showToken.DOLocalMoveZ(Z_OFF_SET, ViewValues.FAST_TIME_ANIMATION))
                .Join(_showToken.DORotate(Camera.main.transform.rotation.eulerAngles * -1f, ViewValues.FAST_TIME_ANIMATION))
                .SetEase(Ease.OutCubic);
        }

        public void OnMouseExit()
        {
            _showToken.DORecolocate(ease: Ease.OutQuad).OnStart(() => _audioComponent.PlayAudio(_hoverOffToken));
        }

        public void OnMouseUpAsButton()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!_isClickable) return;
            _clickHandler.Clicked(this);
        }

        /*******************************************************************/
        public Tween MoveToShowSelector(Transform scenePoint)
        {
            if (!CanBePlayed) return DOTween.Sequence();
            return DOTween.Sequence()
                    .Join(transform.DOMove(ButtonPositionInUI(), ViewValues.DEFAULT_TIME_ANIMATION))
                    .Join(transform.DOScale(scenePoint.lossyScale, ViewValues.DEFAULT_TIME_ANIMATION))
                    .SetEase(Ease.InOutSine);

            Vector3 ButtonPositionInUI()
            {
                float distanceBorderRight = (MOVE_OFFSET - (Screen.width - Camera.main.WorldToScreenPoint(scenePoint.position).x) / Screen.width);
                return new Vector3(scenePoint.position.x - distanceBorderRight, scenePoint.position.y, scenePoint.position.z);
            }
        }

        public Tween RestorePosition() => DOTween.Sequence()
         .Join(transform.DOLocalMove(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
         .Join(transform.DOScale(Vector3.one, ViewValues.DEFAULT_TIME_ANIMATION))
         .Join(transform.DOLocalRotate(Vector3.zero, ViewValues.DEFAULT_TIME_ANIMATION))
         .SetEase(Ease.InOutSine);
    }
}
