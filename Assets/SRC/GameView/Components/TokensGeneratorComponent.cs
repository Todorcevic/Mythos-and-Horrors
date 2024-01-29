using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class TokensGeneratorComponent : MonoBehaviour, IStatableView
    {
        private const float Y_OFF_SET = 1f;
        private const float Z_OFF_SET = 2f;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _showToken;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _tokenCenterShow;
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _resourceTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _hintsTokensView;

        public Transform StatTransform => _showToken;
        public Stat Stat => _chaptersProvider.CurrentScene.ResourcesPile;

        /*******************************************************************/
        public List<TokenView> GetResourceTokens(int amount = 1) => _resourceTokensView.Take(amount).ToList();

        public List<TokenView> GetHintTokens(int amount = 1) => _hintsTokensView.Take(amount).ToList();

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
            _showToken.DOFullMove(_showToken.parent);
        }

        public void OnMouseUpAsButton()
        {
            Debug.Log("Resource Clicked");
        }

        public Tween UpdateValue()
        {
            return DOTween.Sequence();
        }
    }
}
