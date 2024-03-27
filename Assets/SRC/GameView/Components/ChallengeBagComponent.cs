using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeBagComponent : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private List<ChallengeTokenView> _tokensPool;

        /*******************************************************************/
        public async Task DropToken(ChallengeToken realToken)
        {
            ChallengeTokenView tokenView = GetTokenView(realToken);

            tokenView.SetValue(realToken.Value());
            tokenView.gameObject.SetActive(true);
            await tokenView.PushUp();
        }

        private ChallengeTokenView GetTokenView(ChallengeToken tokens)
        {
            ChallengeTokenView tokenPrefab = _tokensPool.First(tokenView => tokenView.Type == tokens.TokenType);
            return Instantiate(tokenPrefab, transform);
        }

    }
}
