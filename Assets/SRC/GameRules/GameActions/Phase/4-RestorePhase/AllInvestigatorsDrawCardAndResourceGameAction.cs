using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class AllInvestigatorsDrawCardAndResourceGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GetLocalizableText("PhaseName_AllInvestigatorsDrawCardAndResource");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_AllInvestigatorsDrawCardAndResource");
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(AllInvestigatorsInPlay, DrawAndGainResource).Execute();
        }

        /*******************************************************************/
        private IEnumerable<Investigator> AllInvestigatorsInPlay() => _investigatorsProvider.AllInvestigatorsInPlay;
        private async Task DrawAndGainResource(Investigator investigator)
        {
            await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Execute();
            await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(investigator, 1).Execute();
        }
    }
}
