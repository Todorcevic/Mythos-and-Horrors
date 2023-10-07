using System;
using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class ZoneRepository
    {
        private List<Zone> _zones;

        /*******************************************************************/
        public Zone GetZone(ZoneType zoneType) => _zones.Find(zone => zone.ZoneType == zoneType);

        public void LoadZones(List<Zone> zones)
        {
            if (_zones != null) throw new ArgumentException(nameof(zones) + " zones already loaded");
            _zones = zones ?? throw new ArgumentNullException(nameof(zones) + " zones cant be null");
        }
    }
}
