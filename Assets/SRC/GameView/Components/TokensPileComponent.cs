using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class TokensPileComponent : MonoBehaviour, IStatableView, IPlayable
    {
        private bool _isClickable;
        private const float Y_OFF_SET = 1f;
        private const float Z_OFF_SET = 2f;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _showToken;
        [SerializeField, Required, ChildGameObjectsOnly] private Light _light;

        public Transform StatTransform => _showToken;
        public Stat Stat => _chaptersProvider.CurrentScene.ResourcesPile;
        public Scene Scene => _chaptersProvider.CurrentScene;
        public bool CanPlayResource => Scene.CanPlayResource;

        List<Effect> IPlayable.EffectsSelected => Scene.PlayableEffects.ToList();

        /*******************************************************************/
        public void ActivateToClick()
        {
            if (!CanPlayResource || _isClickable) return;
            _light.DOIntensity(10f, ViewValues.FAST_TIME_ANIMATION);
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

        Tween IStatableView.UpdateValue() => DOTween.Sequence();
    }
}
