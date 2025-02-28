using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class AllInvestigatorsCheckHandSizeGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public override Phase MainPhase => Phase.Restore;
        public override Localization PhaseNameLocalization => new("PhaseName_AllInvestigatorsCheckHandSize");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_AllInvestigatorsCheckHandSize");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<SafeWhile>().SetWith(GetInvestigatorsMustDiscard, Discard).Execute();
        }

        /*******************************************************************/
        bool GetInvestigatorsMustDiscard() => _investigatorsProvider.AllInvestigatorsInPlay
            .Any(investigator => investigator.HandSize > investigator.MaxHandSize.Value);

        async Task Discard() => await _gameActionsProvider.Create<DiscardMaxHandSizeGameAction>().SetWith(_investigatorsProvider.AllInvestigatorsInPlay
            .FirstOrDefault(investigator => investigator.HandSize > investigator.MaxHandSize.Value)).Execute();
    }
}
