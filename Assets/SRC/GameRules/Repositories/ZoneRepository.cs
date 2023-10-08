using System;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ZoneRepository
    {
        [Inject] private readonly IZonesContainer _zonesContainer;

        private List<Zone> _zones;

        //public List<Zone> PlaceZones { get; set; }
        //public Zone AdventurerZone { get; set; }
        //public Zone AidZone { get; set; }
        //public Zone AdventurerDeckZone { get; set; }
        //public Zone AdventurerDiscardZone { get; set; }
        //public Zone SceneZone { get; set; }
        //public Zone SceneDeckZone { get; set; }
        //public Zone SceneDiscardZone { get; set; }
        //public Zone GoalZone { get; set; }
        //public Zone PlotZone { get; set; }
        //public Zone OutGame { get; set; }
        //public Zone Limbo { get; set; }

        /*******************************************************************/
        public void LoadZones()
        {
            _zones = _zonesContainer.GetZones();
        }

        public Zone GetZone(string zoneName)
        {
            return _zones.Find(zone => zone.CodeName == zoneName)
                   ?? throw new KeyNotFoundException($"Zone {zoneName} not found");
        }
    }
}
