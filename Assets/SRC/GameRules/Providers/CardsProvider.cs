using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class CardsProvider
    {
        public List<Card> AllCards { get; } = new();

        /*******************************************************************/
        public Card GetCard(string code) => AllCards.First(card => card.Info.Code == code);

        public void AddCard(Card objectCard) => AllCards.Add(objectCard);

        public List<Card> GetPlayableCards() => AllCards.FindAll(card => card.CanPlay());
    }
}
