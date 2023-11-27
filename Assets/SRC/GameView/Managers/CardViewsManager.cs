using MythsAndHorrors.EditMode;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.PlayMode
{
    public class CardViewsManager
    {
        [Inject] private readonly List<CardView> _allCardsView;

        /*******************************************************************/

        public CardView Get(Card card) => _allCardsView.First(cardView => cardView.Card == card);
    }
}
