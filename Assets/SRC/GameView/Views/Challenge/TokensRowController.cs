using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class TokensRowController : MonoBehaviour
    {
        private readonly List<SceneTokenView> allTokens = new();
        [SerializeField, Required, AssetsOnly] private ChallengeTokensManager _tokensManager;
        [SerializeField, Required, AssetsOnly] private SceneTokenView _sceneTokenPrefab;
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
            SceneTokenView sceneToken = Instantiate(_sceneTokenPrefab, transform);
            allTokens.Add(sceneToken);
            sceneToken.SetToken(token.Value.Invoke(investigator), token.Description.Invoke(investigator), _tokensManager.GetToken(token.TokenType).Image, _message);
        }
    }
}

