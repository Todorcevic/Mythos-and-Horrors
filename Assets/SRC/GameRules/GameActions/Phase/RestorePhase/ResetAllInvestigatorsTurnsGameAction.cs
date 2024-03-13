using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    //4.2	Reset actions.
    public class ResetAllInvestigatorsTurnsGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionProvider _gameActionProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ResetAllInvestigatorsTurnsGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ResetAllInvestigatorsTurnsGameAction);
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators)
            {
                await _gameActionProvider.Create(new ResetInvestigatorTurnsGameAction(investigator));
            }
        }
    }
}
