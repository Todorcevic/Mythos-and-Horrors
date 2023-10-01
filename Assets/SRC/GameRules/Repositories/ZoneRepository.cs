using System;
using System.Collections.Generic;

namespace Tuesday.GameRules
{
    public class ZoneRepository : IZoneLoader
    {
        private List<Zone> _zones;

        /*******************************************************************/
        public Zone GetZone(ZoneType zoneType) => _zones.Find(zone => zone.ZoneType == zoneType);

        void IZoneLoader.LoadZones(List<Zone> zones)
        {
            if (_zones != null) throw new ArgumentException(nameof(zones) + " zones already loaded");
            _zones = zones ?? throw new ArgumentNullException(nameof(zones) + " zones cant be null");
        }
    }
}
