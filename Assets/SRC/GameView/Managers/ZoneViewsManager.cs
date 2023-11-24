using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZoneViewsManager
    {
        [Inject] private readonly List<ZoneView> _allZones;

        /*******************************************************************/
        public ZoneView Get(Zone zone) => _allZones.Find(zoneView => zoneView.Zone == zone);

        public ZoneView Get(string zoneName) => _allZones.Find(zoneView => zoneView.Zone.CodeName == zoneName);

        public List<Zone> GetSceneZones() => _allZones.Select(zoneView => zoneView.Zone).ToList();
    }
}
