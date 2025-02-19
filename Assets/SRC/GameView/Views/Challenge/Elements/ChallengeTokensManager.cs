using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengeTokensManager : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private ChallengeToken3DView _viewTokenPrefab;
        [SerializeField, Required, AssetsOnly] private ChallengeToken2DView _sceneTokenPrefab;
        [SerializeField, Required, AssetsOnly] private List<ChallengeTokenSO> _tokens;

        /*******************************************************************/
        public ChallengeToken3DView GetTokenView(ChallengeToken challengeToken, Transform transform)
        {
            ChallengeTokenSO tokenSO = _tokens.Find(token => token.TokenType == challengeToken.TokenType);
            ChallengeToken3DView newTokenView = ZenjectHelper.Instantiate(_viewTokenPrefab, transform);
            newTokenView.SetValue(challengeToken, tokenSO.Texture, tokenSO.AudioClip);
            return newTokenView;
        }

        public ChallengeToken2DView GetSceneTokenView(ChallengeToken challengeToken, Transform transform)
        {
            ChallengeTokenSO tokenSO = _tokens.Find(token => token.TokenType == challengeToken.TokenType);
            ChallengeToken2DView newTokenScene = ZenjectHelper.Instantiate(_sceneTokenPrefab, transform);
            newTokenScene.SetImage(tokenSO.Image);
            return newTokenScene;
        }

        public Sprite GetSprite(ChallengeTokenType tokenType)
        {
            ChallengeTokenSO tokenSO = _tokens.Find(token => token.TokenType == tokenType);
            return tokenSO.Image;
        }
    }
}

