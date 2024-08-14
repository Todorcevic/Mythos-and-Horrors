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
        public override string Name => _textsProvider.GetLocalizableText("PhaseName_AllInvestigatorsCheckHandSize");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_AllInvestigatorsCheckHandSize");
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            while (GetInvestigatorsMustDiscard().Any())
            {
                Investigator investigator = GetInvestigatorsMustDiscard().First();
                await _gameActionsProvider.Create<CheckMaxHandSizeGameAction>().SetWith(investigator).Execute();
            }
        }

        /*******************************************************************/
        IEnumerable<Investigator> GetInvestigatorsMustDiscard() => _investigatorsProvider.AllInvestigatorsInPlay
            .Where(investigator => investigator.HandSize > investigator.MaxHandSize.Value);
    }
}
