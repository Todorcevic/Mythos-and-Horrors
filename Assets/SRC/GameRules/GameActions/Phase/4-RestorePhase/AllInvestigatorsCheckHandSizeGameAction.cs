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
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(GetInvestigatorsMustDiscard, Discard).Execute();

            //while (GetInvestigatorsMustDiscard().Any())
            //{
            //    Investigator investigator = GetInvestigatorsMustDiscard().First();
            //    await _gameActionsProvider.Create<DiscardMaxHandSizeGameAction>().SetWith(investigator).Execute();
            //}
        }

        /*******************************************************************/
        IEnumerable<Investigator> GetInvestigatorsMustDiscard() => _investigatorsProvider.AllInvestigatorsInPlay
            .Where(investigator => investigator.HandSize > investigator.MaxHandSize.Value);

        async Task Discard(Investigator investigator) => await _gameActionsProvider.Create<DiscardMaxHandSizeGameAction>().SetWith(investigator).Execute();
    }
}
