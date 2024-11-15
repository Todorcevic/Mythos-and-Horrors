using System;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class Zone
    {
        private readonly List<Card> _cards = new();

        public ZoneType ZoneType { get; }

        public List<Card> Cards => _cards.ToList();
        public Card TopCard => _cards.Last();
        public Card BottomCard => _cards.First();
        public bool IsHandZone => ZoneType == ZoneType.Hand;
        public bool IsDeckZone => ZoneType == ZoneType.InvestigatorDeck;
        public bool IsDiscardZone => ZoneType == ZoneType.InvestigatorDiscard;
        public bool IsAidZone => ZoneType == ZoneType.Aid;
        public bool IsDangerZone => ZoneType == ZoneType.Danger;
        public bool IsInvestigatorZone => ZoneType == ZoneType.Investigator;
        public bool IsPlaceZone => ZoneType == ZoneType.Place;

        /*******************************************************************/
        public Zone(ZoneType zoneType)
        {
            ZoneType = zoneType;
        }

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

        public void ChangePositionOf(Card card, int position)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            if (!_cards.Contains(card)) throw new ArgumentException("Card not in zone", card.Info.Code);
            if (position < 0 || position >= _cards.Count) throw new ArgumentOutOfRangeException(nameof(position));

            _cards.Remove(card);
            _cards.Insert(position, card);
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

        public override string ToString() => ZoneType.ToString();
    }
}
