using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsDrawDangerCardGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public override Phase MainPhase => Phase.Scene;
        public override Localization PhaseNameLocalization => new("PhaseName_InvestigatorsDrawDangerCard");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_InvestigatorsDrawDangerCard");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigatorsInPlay, DrawDangerCard).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> AllInvestigatorsInPlay() => _investigatorsProvider.AllInvestigatorsInPlay;
            async Task DrawDangerCard(Investigator investigator) =>
                     await _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Execute();
        }
    }
}
