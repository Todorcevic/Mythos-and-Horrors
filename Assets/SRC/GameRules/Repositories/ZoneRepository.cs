using System;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ZoneRepository
    {
        [Inject] private readonly IPersistenceZones _zonesContainer;

        private List<Zone> _zones;

        /*******************************************************************/
        public void LoadZones()
        {
            _zones = _zonesContainer.GetZones();
        }

        public Zone GetZone(string zoneName) =>
            _zones.Find(zone => zone.CodeName == zoneName) ?? throw new KeyNotFoundException($"Zone {zoneName} not found");

    }
}
