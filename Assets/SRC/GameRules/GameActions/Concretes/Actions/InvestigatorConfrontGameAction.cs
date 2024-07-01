using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorConfrontGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public CardCreature Creature { get; private set; }

        /*******************************************************************/
        public InvestigatorConfrontGameAction SetWith(Investigator investigator, CardCreature creature)
        {
            Investigator = investigator;
            Creature = creature;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Creature, Investigator.DangerZone).Start();
        }
    }
}
