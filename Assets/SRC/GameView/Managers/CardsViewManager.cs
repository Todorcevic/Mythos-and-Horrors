using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public class CardsViewManager
    {
        private List<CardView> _allCards;

        /*******************************************************************/
        public void Add(CardView cardView) => _allCards.Add(cardView);

        public CardView Get(Card card) => _allCards.Find(cardView => cardView.Card == card);

        public void LoadCardsView(List<CardView> cardsView)
        {
            if (_allCards != null) throw new InvalidOperationException("Cards already loaded");
            _allCards = cardsView ?? throw new ArgumentNullException(nameof(cardsView), "cardsView cant be null");
        }
    }
}
