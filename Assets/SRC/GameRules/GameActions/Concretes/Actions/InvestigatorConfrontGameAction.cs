using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorConfrontGameAction : GameAction
    {
        public Investigator Investigator { get; }
        public CardCreature Creature { get; }

        /*******************************************************************/
        public InvestigatorConfrontGameAction(Investigator investigator, CardCreature creature)
        {
            Investigator = investigator;
            Creature = creature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Creature, Investigator.DangerZone).Start();
        }
    }
}
