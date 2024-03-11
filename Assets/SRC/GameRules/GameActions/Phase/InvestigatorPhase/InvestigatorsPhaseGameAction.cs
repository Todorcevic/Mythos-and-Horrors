using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //2.1	Investigation phase begins.
    public class InvestigatorsPhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        private bool ThereAreInvestigatorsWithTurns => _investigatorsProvider.GetInvestigatorsCanStartTurn.Count > 0;
        public override Phase MainPhase => Phase.Investigator;
        public override string Name => _textsProvider.GameText.INVESTIGATOR_PHASE_NAME;
        public override string Description => _textsProvider.GameText.INVESTIGATOR_PHASE_DESCRIPTION;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            while (ThereAreInvestigatorsWithTurns)
            {
                await _gameActionFactory.Create(new PlayInvestigatorGameAction());
            }
        }
    }
    //2.3	Investigation phase ends.
}
