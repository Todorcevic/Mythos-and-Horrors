using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class CardsProvider
    {
        public List<Card> AllCards { get; } = new();

        /*******************************************************************/
        public Card GetCard(string code) => AllCards.First(card => card.Info.Code == code);

        public T GetCard<T>(string code) where T : Card => AllCards.First(card => card.Info.Code == code && card is T) as T;

        public void AddCard(Card objectCard) => AllCards.Add(objectCard);

        public List<Card> GetPlayableCards() => AllCards.FindAll(card => card.CanPlay);

        public List<Card> GetCardsBuffedWith(IBuffable buff) => AllCards.FindAll(card => card.HasThisBuff(buff));
    }
}
