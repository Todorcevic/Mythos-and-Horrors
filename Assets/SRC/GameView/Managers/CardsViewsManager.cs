using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public class CardsViewsManager
    {
        private List<CardView> _allCardsView;

        /*******************************************************************/
        public void SetCardsView(List<CardView> allCardsView)
        {
            if (_allCardsView != null) throw new InvalidOperationException("Cards already loaded");
            _allCardsView = allCardsView ?? throw new ArgumentNullException(nameof(allCardsView) + " cards cant be null");
        }

        public CardView Get(Card card) => _allCardsView.Find(cardView => cardView.Card == card);
    }
}
