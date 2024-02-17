using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PrepareSceneGameAction : GameAction, IPhase
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Scene Scene { get; }
        string IPhase.Name => throw new System.NotImplementedException();
        string IPhase.Description => throw new System.NotImplementedException();

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
