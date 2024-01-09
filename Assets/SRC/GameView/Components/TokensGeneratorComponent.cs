using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TokensGeneratorComponent : MonoBehaviour
    {
        private const float Y_OFF_SET = 1f;
        private const float Z_OFF_SET = 2f;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _showToken;
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _resourceTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _hintsTokensView;

        public Transform ShowToken => _showToken;

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
    }
}
