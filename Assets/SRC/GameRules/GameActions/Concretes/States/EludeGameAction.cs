using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class EludeGameAction : GameAction
    {

        public CardCreature Creature { get; }
        public Investigator Investigator { get; }

        /*******************************************************************/
        public EludeGameAction(CardCreature creature, Investigator byThisInvestigator)
        {
            Creature = creature;
            Investigator = byThisInvestigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Creature.Exausted, true).Start();
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(Creature, Creature.CurrentPlace.OwnZone).Start();
        }
    }
}
