using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class PrepareSceneGameAction : GameAction
    {
        private Scene _scene;

        /*******************************************************************/
        public async Task Run(Scene scene)
        {
            _scene = scene;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _scene.PrepareScene();
        }
    }
}
