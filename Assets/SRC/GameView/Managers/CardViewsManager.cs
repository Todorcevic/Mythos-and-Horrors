using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardViewsManager
    {
        [Inject] private readonly List<CardView> _allCardsView;

        /*******************************************************************/

        public CardView Get(Card card) => _allCardsView.First(cardView => cardView.Card == card);
    }
}
