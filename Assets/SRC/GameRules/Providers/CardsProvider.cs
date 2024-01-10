using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class CardsProvider
    {
        public List<Card> _allCards = new();
        public CardResource Resource => _allCards.First(card => card is CardResource) as CardResource;
        public List<Card> BuildableCards => _allCards.FindAll(card => !card.IsSpecial);

        /*******************************************************************/
        public Card GetCard(string code) => _allCards.First(card => card.Info.Code == code);

        public void AddCard(Card objectCard) => _allCards.Add(objectCard);

        public List<Card> GetPlayableCards() => BuildableCards.FindAll(card => card.CanPlay());
    }
}
