using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeBagComponent : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private List<ChallengeTokenView> _tokensPool;

        /*******************************************************************/
        public void DropToken(ChallengeToken realToken)
        {
            ChallengeTokenView tokenView = GetTokenView(realToken);

            tokenView.SetValue(realToken.Value());
            tokenView.gameObject.SetActive(true);
            tokenView.PushUp();
        }

        private ChallengeTokenView GetTokenView(ChallengeToken tokens)
        {
            return _tokensPool.FirstOrDefault(tokenView => tokenView.Type == tokens.TokenType);
        }

    }
}
