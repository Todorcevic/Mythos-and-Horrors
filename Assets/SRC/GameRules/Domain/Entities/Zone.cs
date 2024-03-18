using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Zone
    {
        private readonly List<Card> _cards = new();
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public IReadOnlyList<Card> Cards => _cards;
        public Card TopCard => _cards.Last();
        public Card BottomCard => _cards.First();
        public Investigator Owner => _investigatorsProvider.GetInvestigatorWithThisZone(this);
        public bool IsHandZone => Owner?.HandZone == this;
        public bool IsDeckZone => Owner?.DeckZone == this;
        public bool IsDiscardZone => Owner?.DiscardZone == this;
        public bool IsAidZone => Owner?.AidZone == this;
        public bool IsDangerZone => Owner?.DangerZone == this;
        public bool IsInvestigatorZone => Owner?.InvestigatorZone == this;

        /*******************************************************************/
        public void AddCard(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            if (_cards.Contains(card)) throw new ArgumentException("Card already in zone", card.Info.Code);

            _cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            if (!_cards.Contains(card)) throw new ArgumentException("Card not in zone", card.Info.Code);

            _cards.Remove(card);
        }

        public void ReorderCardsWith(IEnumerable<Card> cards)
        {
            if (cards == null) throw new ArgumentNullException(nameof(cards));
            if (!new HashSet<Card>(cards).SetEquals(new HashSet<Card>(_cards))) throw new ArgumentException("Cards not match", nameof(cards));

            _cards.Clear();
            _cards.AddRange(cards);
        }

        public void Shuffle()
        {
            Random rng = new();
            int elementAmount = _cards.Count;
            while (elementAmount > 1)
            {
                elementAmount--;
                int randomNumber = rng.Next(elementAmount + 1);
                (_cards[elementAmount], _cards[randomNumber]) = (_cards[randomNumber], _cards[elementAmount]);
            }
        }

        /*******************************************************************/
        public bool HasThisCard(Card card) => _cards.Contains(card);
    }
}
