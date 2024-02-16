using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class CardViewsManager
    {
        private readonly List<CardView> _allCardsView = new();

        public List<CardView> AllCardsView => _allCardsView;

        /*******************************************************************/
        public CardView GetCardView(Card card) => _allCardsView.First(cardView => cardView.Card == card);

        public List<CardView> GetCardViews(List<Card> cards) => _allCardsView.Where(cardView => cards.Contains(cardView.Card)).ToList();

        public AvatarCardView GetAvatarCardView(Investigator investigator) => GetCardView(investigator.AvatarCard) as AvatarCardView;

        public void AddCardView(CardView cardView) => _allCardsView.Add(cardView);

        public List<CardView> GetAllCanPlay() => _allCardsView.Where(cardView => cardView.Card.CanPlay)
            .OrderBy(cardView => cardView.Card.CurrentZone.Cards.Count).ThenBy(cardView => cardView.DeckPosition).ToList();

        public List<T> GetAllOfType<T>() where T : CardView => _allCardsView.OfType<T>().ToList();

        public List<IUpdatable> GetAllUpdatable() => _allCardsView.OfType<IUpdatable>().ToList();

    }
}
