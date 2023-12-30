using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZoneLoaderUseCase
    {
        [Inject] private readonly SwapInvestigatorComponent _swapInvestigatorComponent;
        [Inject] private readonly AreaSceneView _sceneArea;
        [Inject] private readonly AreaPlacesView _placesArea;

        /*******************************************************************/
        public void Execute()
        {
            _swapInvestigatorComponent.Init();
            _sceneArea.Init();
            _placesArea.Init();
        }
    }
}
