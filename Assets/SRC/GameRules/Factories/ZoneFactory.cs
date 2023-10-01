using System;
using System.Collections.Generic;

namespace GameRules
{
    public class ZoneFactory
    {
        public List<Zone> CreateZones()
        {
            List<Zone> zones = new();
            foreach (ZoneType zoneType in Enum.GetValues(typeof(ZoneType)))
            {
                zones.Add(new Zone(zoneType));
            }
            return zones;
        }
    }
}
