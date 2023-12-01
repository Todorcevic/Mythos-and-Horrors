using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZoneLoaderUseCase
    {
        [Inject] private readonly SwapAdventurerComponent _swapAdventurerComponent;
        [Inject] private readonly AreaSceneView _sceneArea;
        [Inject] private readonly AreaPlacesView _placesArea;

        /*******************************************************************/
        public void Execute()
        {
            _swapAdventurerComponent.Init();
            _sceneArea.Init();
            _placesArea.Init();
        }
    }
}
