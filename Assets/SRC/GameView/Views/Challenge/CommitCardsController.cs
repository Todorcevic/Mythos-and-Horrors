using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class CommitCardsController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<CardChallengeView> _cardChallenges;

        private CardChallengeView GetFreeCardChallenge => _cardChallenges.FirstOrDefault(cardChallenge => cardChallenge.Card == null)
            ?? CreateCardChallenge();

        /*******************************************************************/
        public async Task ShowAll(IEnumerable<ICommitable> commitCards, ChallengeType challengeType)
        {
            _cardChallenges.FindAll(cardChallenge => !commitCards.OfType<Card>().Contains(cardChallenge.Card))
                .ForEach(cardChallenge => cardChallenge.Disable());

            foreach (Card commitCard in commitCards.OfType<Card>().Where(card => !_cardChallenges.Select(cardChallenge => cardChallenge.Card).Contains(card)))
            {
                await GetFreeCardChallenge.SetCard(commitCard, ((ICommitable)commitCard).GetChallengeValue(challengeType));
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

