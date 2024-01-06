using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TokenController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<TokenView> _allTokens;

        private int Amount => _allTokens.Count(tokenView => tokenView.isActiveAndEnabled);
        public TokenView TokenOn => _allTokens.Last(tokenView => tokenView.isActiveAndEnabled);
        public TokenView TokenOff => _allTokens.First(tokenView => !tokenView.isActiveAndEnabled);

        /*******************************************************************/
        public void AddToken(int amoun)
        {
            for (int i = 0; i < amoun; i++) AddToken();
        }

        public void RemoveToken(int amoun)
        {
            for (int i = 0; i < amoun; i++) RemoveToken();
        }

        private void AddToken()
        {
            TokenOff.SetAmount(Amount + 1);
        }

        private void RemoveToken()
        {
            TokenOn.SetAmount(0);
            TokenOn.SetAmount(Amount);
        }
    }
}
