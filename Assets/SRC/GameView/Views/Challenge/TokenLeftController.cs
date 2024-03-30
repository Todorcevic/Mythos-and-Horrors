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
        [SerializeField, Required, AssetsOnly] private ChallengeTokenSO _voidToken;
        [SerializeField, Required, AssetsOnly] private List<ChallengeTokenSO> _tokens;

        public ChallengeTokenSO GetToken(ChallengeTokenType tokenType) =>
            _tokens.Find(token => token.TokenType == tokenType) ?? _voidToken;




        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [SerializeField, Required] private Image _image;

        public void Create()
        {
            foreach (ChallengeTokenType token in _challengeTokensProvider.Tokens)
            {
                Image tokenImage = Instantiate(_image, transform);
                tokenImage.sprite = GetToken(token).Image;
            }
        }
    }
}

