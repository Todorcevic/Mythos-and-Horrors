using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZoneLoaderUseCase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly AreaSceneView _sceneArea;
        [Inject] private readonly AreaPlacesView _placesArea;

        /*******************************************************************/
        public void Execute()
        {
            _areaInvestigatorViewsManager.Init(_investigatorsProvider.AllInvestigators);
            _sceneArea.Init();
            _placesArea.Init();
        }
    }
}
