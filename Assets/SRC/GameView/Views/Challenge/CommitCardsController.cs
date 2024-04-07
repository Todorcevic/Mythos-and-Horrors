using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class CommitCardsController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private ChallengeTokensManager _challengeTokensManager;
        [SerializeField, Required, ChildGameObjectsOnly] private List<CardChallengeView> _cardChallenges;

        private CardChallengeView GetFreeCardChallenge => _cardChallenges.FirstOrDefault(cardChallenge => cardChallenge.Card == null)
            ?? CreateCardChallenge();

        /*******************************************************************/
        public void ShowToken(ChallengeToken challengeToken) =>
            GetFreeCardChallenge.SetToken(challengeToken, _challengeTokensManager.GetToken(challengeToken.TokenType).Image);

        public void RestoreToken(ChallengeToken challengeToken) =>
            _cardChallenges.Find(cardChallenge => cardChallenge.Token == challengeToken)?.Disable();

        public void ClearAll() => _cardChallenges.ForEach(cardChallenge => cardChallenge.Disable());

        public void ShowAll(IEnumerable<ICommitable> commitCards, ChallengeType challengeType)
        {
            _cardChallenges.FindAll(cardChallengeView => !commitCards.OfType<Card>().Contains(cardChallengeView.Card) && cardChallengeView.Token == null)
                .ForEach(cardChallenge => cardChallenge.Disable());

            foreach (Card commitCard in commitCards.OfType<Card>().Where(card => !_cardChallenges.Select(cardChallenge => cardChallenge.Card).Contains(card)))
            {
                GetFreeCardChallenge.SetCard(commitCard, ((ICommitable)commitCard).GetChallengeValue(challengeType));
            }
        }

        private CardChallengeView CreateCardChallenge()
        {
            CardChallengeView cardChallenge = Instantiate(_cardChallenges.First(), transform);
            cardChallenge.Disable();
            _cardChallenges.Add(cardChallenge);
            return cardChallenge;
        }
    }
}

