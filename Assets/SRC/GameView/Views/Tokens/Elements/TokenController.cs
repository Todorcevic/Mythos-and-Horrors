using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TokenController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _allTokens;

        public int Amount => _allTokens.Count(tokenView => tokenView.isActiveAndEnabled);
        public TokenView TokenOn => _allTokens.Last(tokenView => tokenView.isActiveAndEnabled);
        public TokenView TokenOff => _allTokens.Find(tokenView => !tokenView.isActiveAndEnabled) ?? CreateNewToken();

        /*******************************************************************/
        private TokenView CreateNewToken()
        {
            TokenView newToken = _allTokens.Last().Clone();
            _allTokens.Add(newToken);
            return newToken;
        }
    }
}
