using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class MoveCreatureGameAction : GameAction
    {
        public CardCreature Creature { get; private set; }
        public CardPlace Destiny { get; private set; }

        /*******************************************************************/
        public MoveCreatureGameAction SetWith(CardCreature creature, CardPlace destiny)
        {
            Creature = creature;
            Destiny = destiny;
            return this;
        }

        public MoveCreatureGameAction SetWith(IStalker creature, IEnumerable<Investigator> investigatorsInPlay)
        {
            Creature = (CardCreature)creature;
            Destiny = creature.GetPlaceToStalkerMove(investigatorsInPlay);
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Destiny == Creature.CurrentPlace) return;
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Creature, Destiny.OwnZone).Execute();
        }
    }
}
