using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ColosusAttackGameAction : GameAction
    {
        public CardColosus Colosus { get; private set; }
        public override bool CanBeExecuted => Colosus.IsInPlay;

        /*******************************************************************/
        public ColosusAttackGameAction SetWith(CardColosus colosus)
        {
            Colosus = colosus;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsConfronted, Attack).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsConfronted() => Colosus.MassiveInvestigatorsConfronted;

            async Task Attack(Investigator investigator) => await _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(Colosus, investigator).Execute();
        }
    }
}