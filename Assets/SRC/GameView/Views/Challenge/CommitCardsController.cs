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
        public void ClearAll() => _cardChallenges.ForEach(cardChallenge => cardChallenge.Disable());

        public void ShowAll(IEnumerable<CommitableCard> commitCards, ChallengeType challengeType)
        {
            _cardChallenges.FindAll(cardChallengeView => !commitCards.Contains(cardChallengeView.Card)).ForEach(cardChallenge => cardChallenge.Disable());

            foreach (CommitableCard commitCard in commitCards.Where(card => !_cardChallenges.Select(cardChallenge => cardChallenge.Card).Contains(card)))
            {
                GetFreeCardChallenge.SetCard(commitCard, challengeType, commitCard.GetChallengeValue(challengeType));
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

