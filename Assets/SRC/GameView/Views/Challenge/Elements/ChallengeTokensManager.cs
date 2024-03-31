using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "TokensManager", menuName = "ScriptableObjects/ManagerTokens")]
    public class ChallengeTokensManager : ScriptableObject
    {
        [SerializeField, Required, AssetsOnly] private ChallengeTokenSO _voidToken;
        [SerializeField, Required, AssetsOnly] private List<ChallengeTokenSO> _tokens;

        public ChallengeTokenSO GetToken(ChallengeTokenType tokenType) =>
            _tokens.Find(token => token.TokenType == tokenType) ?? _voidToken;
    }
}

