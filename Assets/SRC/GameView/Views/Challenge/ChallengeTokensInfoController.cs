using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengeTokensInfoController : MonoBehaviour
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly TextsManager _textsManager;
        private readonly List<ChallengeToken2DView> allTokensTop = new();
        private readonly List<ChallengeToken2DView> allTokensBottom = new();
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeTokensManager _challengeTokensManager;
        [SerializeField, Required, ChildGameObjectsOnly] private ChallengeMessageController _challengeMessageController;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform topTokensContainer;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform bottomTokensContainer;

        /*******************************************************************/
        public void Init(Investigator investigator)
        {
            DestroyAll();
            SetTopTokens(investigator);
            SetBottomTokens(investigator);
            _challengeMessageController.ResetAll();
        }

        public void ShowRevealedTokens(Investigator investigator)
        {
            allTokensTop.Concat(allTokensBottom).ForEach(token => token.HideToken());
            allTokensTop.Concat(allTokensBottom).Where(challengeToken2d => _challengeTokensProvider.ChallengeTokensRevealed.Contains(challengeToken2d.Challengetoken))
                .ForEach(token => token.ShowToken());

            List<(string, string, Sprite)> allDropTokensInfo = _challengeTokensProvider.ChallengeTokensRevealed.Select(token => (
                token.Value.Invoke(investigator).ToString(),
                token.Description.Invoke(investigator),
                _challengeTokensManager.GetSprite(token.TokenType))).ToList();
            _challengeMessageController.ShowDropTokens(allDropTokensInfo);
        }

        private void SetTopTokens(Investigator investigator)
        {
            foreach (ChallengeToken token in _challengeTokensProvider.AllBasicChallengeTokens.OrderByDescending(challengeToken => (int)challengeToken.TokenType))
            {
                ChallengeToken2DView sceneToken = _challengeTokensManager.GetSceneTokenView(token, topTokensContainer);
                string parseDescription = token.Description.Invoke(investigator).ParseDescription(_cardsProvider, _textsManager, investigator);
                sceneToken.SetToken(token, token.Value.Invoke(investigator), parseDescription, _challengeMessageController);
                allTokensTop.Add(sceneToken);
            }
        }

        private void SetBottomTokens(Investigator investigator)
        {
            foreach (ChallengeToken token in _challengeTokensProvider.AllSpecialChallengeTokens.OrderByDescending(challengeToken => (int)challengeToken.TokenType))
            {
                ChallengeToken2DView sceneToken = _challengeTokensManager.GetSceneTokenView(token, bottomTokensContainer);
                string parseDescription = token.Description.Invoke(investigator).ParseDescription(_cardsProvider, _textsManager, investigator);
                sceneToken.SetToken(token, token.Value.Invoke(investigator), parseDescription, _challengeMessageController);
                allTokensTop.Add(sceneToken);
            }
        }

        private void DestroyAll()
        {
            allTokensTop.ForEach(token => Destroy(token.gameObject));
            allTokensTop.Clear();
            allTokensBottom.ForEach(token => Destroy(token.gameObject));
            allTokensBottom.Clear();
        }
    }
}

