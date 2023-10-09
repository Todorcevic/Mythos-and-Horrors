using System;
using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class ZoneRepository
    {
        private List<Zone> _zones;

        /*******************************************************************/
        public void SetZones(List<Zone> zones)
        {
            if (_zones != null) throw new InvalidOperationException("Zones already loaded");
            _zones = zones ?? throw new ArgumentNullException(nameof(zones) + " zones cant be null");
        }

        public Zone GetZone(string zoneName) =>
            _zones.Find(zone => zone.CodeName == zoneName) ?? throw new KeyNotFoundException($"Zone {zoneName} not found");
    }
}
