using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PrepareSceneGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;

        public Scene Scene { get; private set; }
        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GameText.PREPARE_SCENE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.PREPARE_SCENE_PHASE_DESCRIPTION;

        /*******************************************************************/
        public PrepareSceneGameAction SetWith(Scene scene)
        {
            Scene = scene;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await Scene.PrepareScene();
        }
    }
}
