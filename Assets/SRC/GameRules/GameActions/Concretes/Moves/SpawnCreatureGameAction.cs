using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class SpawnCreatureGameAction : GameAction
    {
        public CardCreature Creature { get; private set; }
        public CardPlace Place { get; private set; }
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public SpawnCreatureGameAction SetWith(ISpawnable spawnable)
        {
            Creature = (CardCreature)spawnable;
            Place = spawnable.SpawnPlace;
            return this;
        }

        public SpawnCreatureGameAction SetWith(CardCreature creature, CardPlace place)
        {
            Creature = creature;
            Place = place;
            return this;
        }

        public SpawnCreatureGameAction SetWith(CardCreature creature, Investigator investigator)
        {
            Creature = creature;
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Place != null) await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Creature, Place.OwnZone).Execute();
            else if (Investigator != null) await _gameActionsProvider.Create<ConfrontCreatureGameAction>().SetWith(Creature, Investigator).Execute();
            else await _gameActionsProvider.Create<DiscardGameAction>().SetWith(Creature).Execute();
        }
    }
}
