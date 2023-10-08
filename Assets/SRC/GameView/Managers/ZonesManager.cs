using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZonesManager : IPersistenceZones
    {
        [Inject] private readonly List<ZoneView> _allZones;

        /*******************************************************************/
        public ZoneView Get(Zone zone) => _allZones.Find(zoneView => zoneView.Zone == zone)
                                          ?? throw new KeyNotFoundException($"Zone {zone} not found");

        public ZoneView Get(string zoneName) => _allZones.Find(zoneView => zoneView.Zone.CodeName == zoneName)
                                                 ?? throw new KeyNotFoundException($"Zone {zoneName} not found");

        List<Zone> IPersistenceZones.GetZones() => _allZones.Select(zoneView => zoneView.Zone).ToList();
    }
}
