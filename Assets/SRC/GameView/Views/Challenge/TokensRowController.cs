using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class TokensRowController : MonoBehaviour
    {
        private readonly List<ChallengeToken2DView> allTokens = new();
        [SerializeField, Required, AssetsOnly] private ChallengeTokensManager _tokensManager;
        [SerializeField, Required] private ChallengeMessageController _challengeMessageController;

        /*******************************************************************/
        public void SetWith(Investigator investigator, IEnumerable<ChallengeToken> tokens)
        {
            DestroyAll();
            tokens.ForEach(token => SetToken(token, investigator));
        }

        private void DestroyAll()
        {
            allTokens.ForEach(token => Destroy(token.gameObject));
            allTokens.Clear();
        }

        private void SetToken(ChallengeToken token, Investigator investigator)
        {
            ChallengeToken2DView sceneToken = _tokensManager.GetSceneTokenView(token, transform);
            sceneToken.SetToken(token, token.Value.Invoke(investigator), token.Description.Invoke(investigator), _challengeMessageController);
            allTokens.Add(sceneToken);
        }

        public void ShowToken(ChallengeToken token)
        {
            allTokens.Find(sceneToken => sceneToken.Challengetoken == token)?.ShowToken();
        }

        public void HideToken(ChallengeToken token)
        {
            allTokens.Find(sceneToken => sceneToken.Challengetoken == token)?.HideToken();
        }
    }
}

