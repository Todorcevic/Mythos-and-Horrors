using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorLoopGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(PlayInvestigatorLoopGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(PlayInvestigatorLoopGameAction);
        public override Phase MainPhase => Phase.Investigator;

        public override bool CanBeExecuted => ActiveInvestigator.HasTurnsAvailable;

        /*******************************************************************/
        public PlayInvestigatorLoopGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(ActiveInvestigator));
        }
    }
}
