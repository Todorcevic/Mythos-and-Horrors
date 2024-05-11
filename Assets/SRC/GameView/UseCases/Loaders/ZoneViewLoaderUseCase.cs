using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ZoneViewLoaderUseCase
    {
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly AreaSceneView _sceneArea;
        [Inject] private readonly AreaPlacesView _placesArea;

        /*******************************************************************/
        public void Execute()
        {
            _areaInvestigatorViewsManager.Init();
            _sceneArea.Init();
            _placesArea.Init();
        }
    }
}
