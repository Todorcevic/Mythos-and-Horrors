using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class OpportunityAttackGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public override bool CanBeExecuted => Investigator.AllTypeCreaturesConfronted.Any();

        /*******************************************************************/
        public OpportunityAttackGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<CardCreature>>().SetWith(AllCreaturesConfronted, CreatureAttack).Start();

            /*******************************************************************/
            IEnumerable<CardCreature> AllCreaturesConfronted() => Investigator.AllTypeCreaturesConfronted;
            async Task CreatureAttack(CardCreature creature) =>
                await _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(creature, Investigator).Start();
        }
    }
}
