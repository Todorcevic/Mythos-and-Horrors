using System;
using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class Zone
    {
        public List<Card> Cards { get; } = new();

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
