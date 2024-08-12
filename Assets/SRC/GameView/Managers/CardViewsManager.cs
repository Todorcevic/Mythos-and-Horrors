using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameView
{
    public class CardViewsManager
    {
        private readonly List<CardView> _allCardsView = new();

        public List<CardView> AllCardsView => _allCardsView;

        /*******************************************************************/
        public void AddCardView(CardView cardView) => _allCardsView.Add(cardView);

        public CardView GetCardView(string cardId) => _allCardsView.First(cardView => cardView.Card.Info.Code == cardId);

        public CardView GetCardView(Card card) => _allCardsView.First(cardView => cardView.Card == card);

        public CardView GetAvatarCardView(Investigator investigator) => GetCardView(investigator.AvatarCard);

        public List<CardView> GetAllCanPlay() => _allCardsView.FindAll(cardView => cardView.Card.CanBePlayed);

        public List<IUpdatable> GetAllUpdatable() => _allCardsView.OfType<IUpdatable>().ToList();

        public List<IPlayable> GetAllIPlayable() => _allCardsView.OfType<IPlayable>().ToList();
    }
}
