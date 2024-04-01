using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class TokenLeftController : MonoBehaviour
    {
        private readonly List<Image> allTokens = new();
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [SerializeField, Required, AssetsOnly] private ChallengeTokensManager _tokensManager;
        [SerializeField, Required, AssetsOnly] private Image _tokenLeftPrefab;

        public void Refresh()
        {
            DestroyAll();
            foreach (ChallengeTokenType token in _challengeTokensProvider.Tokens)
            {
                Image tokenImage = Instantiate(_tokenLeftPrefab, transform);
                allTokens.Add(tokenImage);
                tokenImage.sprite = _tokensManager.GetToken(token).Image;
            }
        }

        private void DestroyAll()
        {
            allTokens.ForEach(token => Destroy(token.gameObject));
            allTokens.Clear();
        }
    }
}

