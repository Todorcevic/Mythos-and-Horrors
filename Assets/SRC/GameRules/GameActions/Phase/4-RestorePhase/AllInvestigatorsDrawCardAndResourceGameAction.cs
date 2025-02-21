using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class AllInvestigatorsDrawCardAndResourceGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public override Phase MainPhase => Phase.Restore;
        public override Localization PhaseNameLocalization => new("PhaseName_AllInvestigatorsDrawCardAndResource");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_AllInvestigatorsDrawCardAndResource");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigatorsInPlay, DrawAndGainResource).Execute();
        }

        /*******************************************************************/
        private IEnumerable<Investigator> AllInvestigatorsInPlay() => _investigatorsProvider.AllInvestigatorsInPlay;
        private async Task DrawAndGainResource(Investigator investigator)
        {
            await _gameActionsProvider.Create<InvestigatorDrawAndGainTokenGameAction>().SetWith(investigator).Execute();
        }
    }
}
