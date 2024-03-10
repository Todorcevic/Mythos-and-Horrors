using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class CardsProvider
    {
        public List<Card> AllCards { get; } = new();

        /*******************************************************************/
        public Card GetCard(string code) => AllCards.First(card => card.Info.Code == code);
        public T GetCard<T>(string code) where T : Card => AllCards.First(card => card.Info.Code == code && card is T) as T;
        public void AddCard(Card objectCard) => AllCards.Add(objectCard);
        public Card GetCardWithThisZone(Zone zone) => AllCards.Find(card => card.OwnZone == zone);
        public List<CardPlace> GetCardsThatCanMoveTo(CardPlace cardPlace) =>
            AllCards.OfType<CardPlace>().Where(place => place.ConnectedPlacesToMove.Contains(cardPlace)).ToList();
    }
}
