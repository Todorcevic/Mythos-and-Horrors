using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PrepareSceneGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Scene Scene { get; }

        /*******************************************************************/
        public PrepareSceneGameAction(Scene scene)
        {
            Scene = scene;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new ShowHistoryGameAction(Scene.Info.Description));
            await Scene.PrepareScene();
        }
    }
}
