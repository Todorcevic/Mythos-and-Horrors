using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class PrepareSceneGameAction : GameAction
    {
        public Scene Scene { get; }

        /*******************************************************************/
        public PrepareSceneGameAction(Scene scene)
        {
            Scene = scene;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await Scene.PrepareScene();
        }
    }
}
