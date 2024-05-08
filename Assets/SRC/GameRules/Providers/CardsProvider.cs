using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class CardsProvider
    {
        private readonly List<Card> _allCards = new();

        public List<Card> AllCards => _allCards.ToList();
        public IEnumerable<CardCreature> AttackerCreatures => _allCards.OfType<CardCreature>()
                  .Where(creature => creature.IsConfronted && !creature.Exausted.IsActive)
                  .OrderBy(creature => creature.ConfrontedInvestigator.Position);
        public IEnumerable<IStalker> StalkersInPlay => _allCards.OfType<IStalker>().Where(stalker => stalker.CurrentPlace != null);

        /*******************************************************************/
        public void AddCard(Card objectCard) => _allCards.Add(objectCard);

        public T GetCard<T>() where T : Card => _allCards.OfType<T>().First();
        public IEnumerable<T> GetCards<T>() where T : Card => _allCards.OfType<T>();

        public Card GetCardByCode(string code) => _allCards.First(card => card.Info.Code == code);

        public Card GetCardWithThisZone(Zone zone) => _allCards.Find(card => card.OwnZone == zone);
        public Card GetCardWithThisStat(Stat stat) => _allCards.Find(card => card.HasThisStat(stat));

        public IEnumerable<CardPlace> GetCardsThatCanMoveTo(CardPlace cardPlace) =>
            _allCards.OfType<CardPlace>().Where(place => place.ConnectedPlacesToMove.Contains(cardPlace));

        public IEnumerable<Card> GetCardsExhausted() => _allCards.Where(card => card.Exausted.IsActive);

        public IEnumerable<Card> GetCardsInPlay() => _allCards.Where(card => card.IsInPlay);
    }
}
