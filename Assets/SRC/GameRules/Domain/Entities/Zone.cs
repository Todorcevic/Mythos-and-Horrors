using System;
using System.Collections.Generic;

namespace MythsAndHorrors.EditMode
{
    public class Zone
    {
        private readonly List<Card> cards = new();

        /*******************************************************************/
        public void AddCard(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            if (cards.Contains(card)) throw new ArgumentException("Card already in zone", card.Info.Code);

            cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            if (!cards.Contains(card)) throw new ArgumentException("Card not in zone", card.Info.Code);

            cards.Remove(card);
        }
    }
}
