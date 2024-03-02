using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Zone
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public List<Card> Cards { get; } = new();
        public Card TopCard => Cards.Last();
        public Card BottomCard => Cards.First();
        public Investigator Owner => _investigatorsProvider.GetInvestigatorWithThisZone(this);
        public bool IsHandZone => Owner?.HandZone == this;

        /*******************************************************************/
        public void AddCard(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            if (Cards.Contains(card)) throw new ArgumentException("Card already in zone", card.Info.Code);

            Cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            if (!Cards.Contains(card)) throw new ArgumentException("Card not in zone", card.Info.Code);

            Cards.Remove(card);
        }
    }
}
