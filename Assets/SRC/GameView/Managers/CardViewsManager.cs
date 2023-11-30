using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class CardViewsManager
    {
        private readonly List<CardView> _allCardsView = new();

        /*******************************************************************/
        public CardView Get(Card card) => _allCardsView.First(cardView => cardView.Card == card);

        public void Add(CardView cardView) => _allCardsView.Add(cardView);
    }
}
