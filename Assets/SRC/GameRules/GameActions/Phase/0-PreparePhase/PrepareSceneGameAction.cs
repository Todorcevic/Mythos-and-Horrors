using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PrepareSceneGameAction : PhaseGameAction
    {
        public Scene Scene { get; private set; }
        public override Phase MainPhase => Phase.Prepare;
        public override Localization PhaseNameLocalization => new("PhaseName_PrepareScene");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_PrepareScene");

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
