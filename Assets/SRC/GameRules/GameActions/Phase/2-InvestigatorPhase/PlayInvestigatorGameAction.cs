using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction
    {
        private bool isStop;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(PlayInvestigatorGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(PlayInvestigatorGameAction);
        public override Phase MainPhase => Phase.Investigator;

        public override bool CanBeExecuted => ActiveInvestigator?.HasTurnsAvailable ?? false;

        /*******************************************************************/
        public PlayInvestigatorGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            while (CanBeExecuted && !isStop)
            {
                await _gameActionsProvider.Create(new OneInvestigatorTurnGameAction(ActiveInvestigator));
            }
        }

        /*******************************************************************/
        public void Stop()
        {
            isStop = true;
        }
    }
}
