using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class TokensPileComponent : MonoBehaviour, IStatableView
    {
        private const float Y_OFF_SET = 1f;
        private const float Z_OFF_SET = 2f;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _showToken;

        public Transform StatTransform => _showToken;
        public Stat Stat => _chaptersProvider.CurrentScene.ResourcesPile;

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
            Debug.Log("Resource Clicked");
        }

        Tween IStatableView.UpdateValue()
        {
            return DOTween.Sequence();
        }
    }
}
