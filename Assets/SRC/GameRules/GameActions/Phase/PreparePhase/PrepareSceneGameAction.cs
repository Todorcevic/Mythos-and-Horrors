using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PrepareSceneGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly TextsProvider _textsProvider;

        public Scene Scene { get; }
        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GameText.PREPARE_SCENE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.PREPARE_SCENE_PHASE_DESCRIPTION;

        /*******************************************************************/
        public PrepareSceneGameAction(Scene scene)
        {
            Scene = scene;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionFactory.Create(new ShowHistoryGameAction(Scene.Info.Description));
            await Scene.PrepareScene();
        }
    }
}
