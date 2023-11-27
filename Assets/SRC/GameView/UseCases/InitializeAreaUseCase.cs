using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InitializeAreaUseCase
    {
        [Inject] private readonly SwapAdventurerComponent _swapAdventurerComponent;
        [Inject] private readonly AreaSceneView _sceneArea;
        [Inject] private readonly AreaPlacesView _placesArea;

        /*******************************************************************/
        public void Execute()
        {
            BuildAdventurerAreas();
            BuildSceneArea();
            BuildPlacesArea();
        }

        private void BuildAdventurerAreas() => _swapAdventurerComponent.Init();

        private void BuildSceneArea() => _sceneArea.Init();

        private void BuildPlacesArea() => _placesArea.Init();
    }
}
