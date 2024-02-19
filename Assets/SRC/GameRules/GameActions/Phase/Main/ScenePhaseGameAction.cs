using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ScenePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public override Phase MainPhase => Phase.Scene;
        public override string Name => _textsProvider.GameText.SCENE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.SCENE_PHASE_DESCRIPTION;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionFactory.Create(new InteractableGameAction(false));
        }
    }
}
