using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class AllInvestigatorsCheckHandSize : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(AllInvestigatorsCheckHandSize);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(AllInvestigatorsCheckHandSize);
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
