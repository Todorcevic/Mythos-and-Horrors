using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PrepareSceneGameAction : PhaseGameAction
    {
        public Scene Scene { get; private set; }
        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GetLocalizableText("PhaseName_PrepareScene");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_PrepareScene");

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
