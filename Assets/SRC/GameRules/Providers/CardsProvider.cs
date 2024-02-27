using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public class CardsProvider
    {
        public List<Card> AllCards { get; } = new();

        /*******************************************************************/
        public Card GetCard(string code) => AllCards.First(card => card.Info.Code == code);
        public T GetCard<T>(string code) where T : Card => AllCards.First(card => card.Info.Code == code && card is T) as T;
        public void AddCard(Card objectCard) => AllCards.Add(objectCard);
        public List<IEffectable> GetPlayableCards() => AllCards.FindAll(card => card.CanPlay).OfType<IEffectable>().ToList();
        public List<Card> GetCardsBuffedWith(IBuffable buff) => AllCards.FindAll(card => card.HasThisBuff(buff));
        public Card GetCardWithThisZone(Zone zone) => AllCards.Find(card => card.OwnZone == zone);
        public List<CardPlace> GetCardsThatCanMoveTo(CardPlace cardPlace) =>
            AllCards.OfType<CardPlace>().Where(place => place.ConnectedPlacesToMove.Contains(cardPlace)).ToList();
    }
}
