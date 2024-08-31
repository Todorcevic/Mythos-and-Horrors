using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class TokensRowController : MonoBehaviour
    {
        private readonly List<ChallengeToken2DView> allTokens = new();
        [SerializeField, Required, AssetsOnly] private ChallengeTokensManager _tokensManager;
        [SerializeField, Required] private TextMeshProUGUI _message;

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
            sceneToken.SetToken(token.Value.Invoke(investigator), token.Description.Invoke(investigator), _message);
            allTokens.Add(sceneToken);
        }
    }
}

