using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TokensGeneratorComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _resourceTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _hintsTokensView;

        /*******************************************************************/
        public List<TokenView> GetResourceTokens(int amount = 1) => _resourceTokensView.Take(amount).ToList();

        public List<TokenView> GetHintTokens(int amount = 1) => _hintsTokensView.Take(amount).ToList();
    }
}
