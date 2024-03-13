using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //4.5	Each investigator checks hand size.
    public class AllInvestigatorsCheckHandSize : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionProvider _gameActionProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(AllInvestigatorsCheckHandSize);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(AllInvestigatorsCheckHandSize);
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                await _gameActionProvider.Create(new CheckHandSizeGameAction(investigator));
            }
        }
    }
}
