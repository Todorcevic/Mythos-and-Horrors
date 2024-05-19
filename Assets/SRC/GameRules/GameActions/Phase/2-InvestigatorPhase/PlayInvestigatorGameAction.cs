using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        private Investigator lastInvestigator;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(PlayInvestigatorGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(PlayInvestigatorGameAction);
        public override Phase MainPhase => Phase.Investigator;

        public override bool CanBeExecuted => ActiveInvestigator.HasTurnsAvailable;
        public static Investigator PlayActiveInvestigator { get; private set; }

        /*******************************************************************/
        public PlayInvestigatorGameAction(Investigator investigator)
        {
            lastInvestigator = PlayActiveInvestigator;
            PlayActiveInvestigator = ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new OneInvestigatorTurnGameAction());
        }

        public override async Task Undo()
        {
            PlayActiveInvestigator = lastInvestigator;
            await base.Undo();
        }
    }
}
