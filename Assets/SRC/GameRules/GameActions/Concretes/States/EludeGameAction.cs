using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class EludeGameAction : GameAction
    {
        public CardCreature Creature { get; private set; }
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public EludeGameAction SetWith(CardCreature creature, Investigator byThisInvestigator)
        {
            Creature = creature;
            Investigator = byThisInvestigator;
            return this;
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
