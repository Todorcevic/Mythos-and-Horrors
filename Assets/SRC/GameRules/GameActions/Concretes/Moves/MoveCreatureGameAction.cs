using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveCreatureGameAction : GameAction
    {

        public CardCreature Creature { get; }
        public CardPlace Destiny { get; }

        /*******************************************************************/
        public MoveCreatureGameAction(CardCreature creature, CardPlace destiny)
        {
            Creature = creature;
            Destiny = destiny;
        }

        public MoveCreatureGameAction(IStalker creature, IEnumerable<Investigator> investigatorsInPlay)
        {
            Creature = (CardCreature)creature;
            Destiny = creature.GetPlaceToStalkerMove(investigatorsInPlay);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Destiny == Creature.CurrentPlace) return;
            await _gameActionsProvider.Create(new MoveCardsGameAction(Creature, Destiny.OwnZone));
        }
    }
}
