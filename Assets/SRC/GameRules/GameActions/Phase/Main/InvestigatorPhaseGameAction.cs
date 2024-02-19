using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InvestigatorPhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public override Phase MainPhase => Phase.Investigator;
        public override string Name => _textsProvider.GameText.INVESTIGATOR_PHASE_NAME;
        public override string Description => _textsProvider.GameText.INVESTIGATOR_PHASE_DESCRIPTION;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionFactory.Create(new InteractableGameAction(false));
        }
    }
}
