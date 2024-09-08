using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardsProvider
    {
        [Inject] private readonly OwnersProvider _ownersProvider;

        public IEnumerable<Card> AllCards => _ownersProvider.AllOwners.SelectMany(owner => owner.Cards);
        public IEnumerable<CardCreature> AttackerCreatures => AllCards.OfType<CardCreature>()
                  .Where(creature => creature.IsConfronted && !creature.Exausted.IsActive)
                  .OrderBy(creature => creature.ConfrontedInvestigator?.Position);

        /*******************************************************************/
        public T GetCard<T>() where T : Card => AllCards.OfType<T>().First();
        public T TryGetCard<T>() where T : Card => AllCards.OfType<T>().FirstOrDefault();
        public IEnumerable<T> GetCards<T>() where T : Card => AllCards.OfType<T>();

        public Card GetCardByCode(string code) => AllCards.First(card => card.Info.Code == code);
        public Card TryGetCardByCode(string code) => AllCards.FirstOrDefault(card => card.Info.Code == code);

        public Card GetCardWithThisZone(Zone zone) => AllCards.FirstOrDefault(card => card.OwnZone == zone);
        public Card GetCardWithThisStat(Stat stat) => AllCards.FirstOrDefault(card => card.HasThisStat(stat));

        public IEnumerable<CardPlace> GetCardsThatCanMoveTo(CardPlace cardPlace) =>
            AllCards.OfType<CardPlace>().Where(place => place.ConnectedPlacesToMove.Contains(cardPlace));

        public IEnumerable<Card> GetCardsExhausted() => AllCards.Where(card => card.Exausted.IsActive);

        public IEnumerable<Card> GetCardsInPlay() => AllCards.Where(card => card.IsInPlay.IsTrue);
    }
}
