using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PrepareSceneGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;

        public Scene Scene { get; }

        /*******************************************************************/
        public PrepareSceneGameAction(Scene scene)
        {
            Scene = scene;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _viewLayerProvider.PlayAnimationWith(this);
            await Scene.PrepareScene();
        }
    }
}
