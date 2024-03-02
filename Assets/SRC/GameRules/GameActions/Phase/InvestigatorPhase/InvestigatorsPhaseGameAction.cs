using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InvestigatorsPhaseGameAction : PhaseGameAction //2.1	Investigation phase begins.
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        private bool ThereAreInvestigatorsWithTurns => _investigatorsProvider.GetInvestigatorsCanStart.Count > 0;
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
}
