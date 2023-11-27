using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZoneViewsManager
    {
        [Inject] private readonly List<ZoneView> _allZones;
        [Inject] private readonly SwapAdventurerComponent _swapAdventurerComponent;
        [Inject] private readonly AreaSceneView _sceneArea;
        [Inject] private readonly AreaPlacesView _placesArea;

        /*******************************************************************/
        public void Init()
        {
            _swapAdventurerComponent.Init();
            _sceneArea.Init();
            _placesArea.Init();
        }

        /*******************************************************************/
        public ZoneView Get(Zone zone) => _allZones.First(zoneView => zoneView.Zone == zone);

        public ZoneView Get(string zoneName) => _allZones.First(zoneView => zoneView.name == zoneName);
    }
}
