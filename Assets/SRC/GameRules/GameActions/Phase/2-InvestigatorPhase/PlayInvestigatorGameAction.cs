using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        private Investigator lastInvestigator;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(PlayInvestigatorGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(PlayInvestigatorGameAction);
        public override Phase MainPhase => Phase.Investigator;

        public override bool CanBeExecuted => ActiveInvestigator.HasTurnsAvailable;
        public static Investigator PlayActiveInvestigator { get; private set; }

        /*******************************************************************/
        public PlayInvestigatorGameAction SetWith(Investigator investigator)
        {
            lastInvestigator = PlayActiveInvestigator;
            PlayActiveInvestigator = ActiveInvestigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActiveInvestigator.IsPlayingTurns, true).Start();
            await _gameActionsProvider.Create<OneInvestigatorTurnGameAction>().SetWith().Start();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActiveInvestigator.IsPlayingTurns, false).Start();
        }

        public override async Task Undo()
        {
            PlayActiveInvestigator = lastInvestigator;
            await base.Undo();
        }
    }
}
