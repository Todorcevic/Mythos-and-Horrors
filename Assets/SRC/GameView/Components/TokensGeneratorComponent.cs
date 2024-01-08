using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TokensGeneratorComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _showToken;
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _resourceTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _hintsTokensView;

        /*******************************************************************/
        public List<TokenView> GetResourceTokens(int amount = 1) => _resourceTokensView.Take(amount).ToList();

        public List<TokenView> GetHintTokens(int amount = 1) => _hintsTokensView.Take(amount).ToList();

        public void OnMouseEnter()
        {
            _showToken.DORotate(new Vector3(-45, 0, 0), ViewValues.FAST_TIME_ANIMATION);
            _showToken.DOLocalMoveY(1, ViewValues.FAST_TIME_ANIMATION);
            _showToken.DOLocalMoveZ(2, ViewValues.FAST_TIME_ANIMATION);
        }

        public void OnMouseExit()
        {
            _showToken.DOFullMove(_showToken.parent);
        }
    }
}
