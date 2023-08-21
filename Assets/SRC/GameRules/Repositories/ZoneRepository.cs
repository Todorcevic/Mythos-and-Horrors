using System;
using System.Collections.Generic;

namespace GameRules
{
    public class ZoneRepository
    {
        private readonly List<Zone> _zones = new();

        /*******************************************************************/
        public void CreateZones()
        {
            foreach (ZoneType zoneType in Enum.GetValues(typeof(ZoneType)))
            {
                _zones.Add(new Zone(zoneType));
            }
        }
        /*******************************************************************/

        public Zone GetZone(ZoneType zoneType) => _zones.Find(zone => zone.ZoneType == zoneType);

    }
}
