using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class CardsProvider
    {
        public List<Card> AllCards { get; } = new();
        public CardResource Resource => AllCards.First(card => card is CardResource) as CardResource;
        public List<Card> BuildableCards => AllCards.FindAll(card => !card.IsSpecial);

        /*******************************************************************/
        public Card GetCard(string code) => AllCards.First(card => card.Info.Code == code);

        public void AddCard(Card objectCard)
        {
            AllCards.Add(objectCard);
        }

        public List<Card> PlayabledCards() => BuildableCards.FindAll(card => card.CanPlay());
    }
}
