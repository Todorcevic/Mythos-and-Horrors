using System;
using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class ZoneProvider
    {
        private List<Zone> _zones;

        public Zone SelectorZone { get; } = new Zone("SelectorZone");

        /*******************************************************************/
        public void AddZone(Zone zone)
        {
            if (_zones == null) throw new InvalidOperationException("Zones not loaded");
            if (zone == null) throw new ArgumentNullException(nameof(zone) + " zone cant be null");
            if (_zones.Contains(zone)) throw new InvalidOperationException("Zone already added");
            _zones.Add(zone);
        }

        public void SetZones(List<Zone> zones)
        {
            if (_zones != null) throw new InvalidOperationException("Zones already loaded");
            _zones = zones ?? throw new ArgumentNullException(nameof(zones) + " zones cant be null");
        }

        public Zone GetZone(string zoneName) =>
            _zones.Find(zone => zone.CodeName == zoneName) ?? throw new KeyNotFoundException($"Zone {zoneName} not found");
    }
}
