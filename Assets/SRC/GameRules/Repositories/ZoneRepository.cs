using System;
using System.Collections.Generic;

namespace GameRules
{
    public class ZoneRepository
    {
        private readonly List<Zone> _zones = new();

        /*******************************************************************/
        public void AddZone(Zone zone) => _zones.Add(zone);

        public Zone GetZone(ZoneType zoneType) => _zones.Find(zone => zone.ZoneType == zoneType);

    }
}
