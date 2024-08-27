using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        private Investigator lastInvestigator;

        public override string Name => _textsProvider.GetLocalizableText("PhaseName_PlayInvestigator");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_PlayInvestigator");
        public override Phase MainPhase => Phase.Investigator;

        public override bool CanBeExecuted => ActiveInvestigator.HasTurnsAvailable.IsTrue;
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
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActiveInvestigator.IsPlayingTurns, true).Execute();
            await _gameActionsProvider.Create<OneInvestigatorTurnGameAction>().SetWith().Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActiveInvestigator.IsPlayingTurns, false).Execute();
        }

        public override async Task Undo()
        {
            PlayActiveInvestigator = lastInvestigator;
            await base.Undo();
        }
    }
}
