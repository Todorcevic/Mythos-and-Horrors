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
        public ZoneView Get(Zone zone) => _allZones.First(zoneView => zoneView.Zone == zone);

        public ZoneView Get(string zoneName) => _allZones.First(zoneView => zoneView.name == zoneName);
    }
}
