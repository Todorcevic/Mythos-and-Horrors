using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class CardsProvider
    {
        public List<Card> AllCards { get; } = new();

        public IEnumerable<CardCreature> AttackerCreatures => AllCards.OfType<CardCreature>()
                  .Where(creature => creature.IsConfronted && !creature.Exausted.IsActive)
                  .OrderBy(creature => creature.ConfrontedInvestigator.Position);
        public IEnumerable<IStalker> StalkersInPlay => AllCards.OfType<IStalker>().Where(stalker => stalker.CurrentPlace != null);

        /*******************************************************************/
        public void AddCard(Card objectCard) => AllCards.Add(objectCard);

        public Card GetCard(string code) => AllCards.First(card => card.Info.Code == code);

        public T GetCard<T>(string code) where T : Card => (T)AllCards.First(card => card.Info.Code == code);

        public Card GetCardWithThisZone(Zone zone) => AllCards.Find(card => card.OwnZone == zone);

        public IEnumerable<CardPlace> GetCardsThatCanMoveTo(CardPlace cardPlace) =>
            AllCards.OfType<CardPlace>().Where(place => place.ConnectedPlacesToMove.Contains(cardPlace));

        public IEnumerable<Card> GetCardsExhausted() => AllCards.FindAll(card => card.Exausted.IsActive);


    }
}
