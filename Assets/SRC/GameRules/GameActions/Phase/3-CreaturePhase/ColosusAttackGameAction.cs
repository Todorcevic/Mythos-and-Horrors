using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ColosusAttackGameAction : GameAction
    {
        public CardColosus Colosus { get; }

        /*******************************************************************/
        public ColosusAttackGameAction(CardColosus colosus)
        {
            Colosus = colosus;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsConfronted, Attack).Start();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsConfronted() => Colosus.MassiveInvestigatorsConfronted;

            async Task Attack(Investigator investigator) => await _gameActionsProvider.Create(new CreatureAttackGameAction(Colosus, investigator));
        }
    }
}