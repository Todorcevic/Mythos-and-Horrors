﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class TokensPileComponent : MonoBehaviour, IStatableView, IPlayable
    {
        private bool _isClickable;
        private const float MOVE_OFFSET = 8;
        private const float Y_OFF_SET = 1f;
        private const float Z_OFF_SET = 2f;
        private const float LIGHT_INTENSITY = 10f;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _showToken;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;

        public IEffectable Resources => _chaptersProvider.CurrentScene;
        List<Effect> IPlayable.EffectsSelected => Resources.PlayableEffects;

        /*******************************************************************/
        Transform IStatableView.StatTransform => _showToken;
        Stat IStatableView.Stat => _chaptersProvider.CurrentScene.PileAmount;
        Tween IStatableView.UpdateValue() => DOTween.Sequence();

        /*******************************************************************/
        public void ActivateToClick()
        {
            if (_isClickable || !Resources.CanBePlayed) return;
            _light.DOIntensity(LIGHT_INTENSITY, ViewValues.FAST_TIME_ANIMATION);
            _isClickable = true;
        }

        public void DeactivateToClick()
        {
            if (!_isClickable) return;
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION);
            _isClickable = false;
        }

        /*******************************************************************/
        public void OnMouseEnter()
        {
            DOTween.Sequence()
                .Join(_showToken.DOLocalMoveY(Y_OFF_SET, ViewValues.FAST_TIME_ANIMATION))
                .Join(_showToken.DOLocalMoveZ(Z_OFF_SET, ViewValues.FAST_TIME_ANIMATION))
                .Join(_showToken.DORotate(Camera.main.transform.rotation.eulerAngles * -1f, ViewValues.FAST_TIME_ANIMATION))
                .SetEase(Ease.OutCubic);
        }

        public void OnMouseExit()
        {
            _showToken.DORecolocate(ease: Ease.OutQuad);
        }

        public void OnMouseUpAsButton()
        {
            if (!_isClickable) return;
            _clickHandler.Clicked(this);
        }

        /*******************************************************************/
        public Tween MoveToShowSelector(Transform scenePoint)
        {
            if (!Resources.CanBePlayed) return DOTween.Sequence();
            return DOTween.Sequence()
                    .Join(transform.DOMove(ButtonPositionInUI(), ViewValues.DEFAULT_TIME_ANIMATION))
                    .Join(transform.DOScale(scenePoint.lossyScale, ViewValues.DEFAULT_TIME_ANIMATION))
                    .SetEase(Ease.InOutCubic);

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
         .SetEase(Ease.InSine);
    }
}