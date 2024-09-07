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
        private const float LIGHT_INTENSITY = 10f;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _showToken;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;

        private CardEffect TakeResourceEffect => (_gameActionsProvider.CurrentInteractable as InvestigatorTurnGameAction)?.TakeResourceEffect;
        IEnumerable<BaseEffect> IPlayable.EffectsSelected => TakeResourceEffect == null ? Enumerable.Empty<CardEffect>() : new[] { TakeResourceEffect };

        /*******************************************************************/
        Transform IStatable.StatTransform => _showToken;
        Stat IStatable.Stat => _chaptersProvider.CurrentScene.PileAmount;
        Tween IStatable.UpdateAnimation() => DOTween.Sequence();

        /*******************************************************************/
        public void ActivateToClick()
        {
            if (_isClickable || !((IPlayable)this).CanBePlayed) return;
            _light.DOIntensity(LIGHT_INTENSITY, ViewValues.FAST_TIME_ANIMATION).OnComplete(() => _isClickable = true);

        }

        public void DeactivateToClick()
        {
            if (!_isClickable) return;
            _light.DOIntensity(0f, ViewValues.FAST_TIME_ANIMATION).OnComplete(() => _isClickable = false);
        }

        /*******************************************************************/
        public void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

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
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!_isClickable) return;
            _clickHandler.Clicked(this);
        }

        /*******************************************************************/
        public Tween MoveToShowSelector(Transform scenePoint)
        {
            if (!((IPlayable)this).CanBePlayed) return DOTween.Sequence();
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
