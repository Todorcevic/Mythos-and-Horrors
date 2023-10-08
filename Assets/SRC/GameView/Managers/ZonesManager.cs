using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZonesManager : IZonesContainer
    {
        [Inject] private readonly List<ZoneView> _allZones;

        //[Inject(Id = "AdventurerZone")] public ZoneBasicView AdventurerZone { get; private set; }
        //[Inject(Id = "AdventurerDeckZone")] public ZoneDeckView AdventurerDeckZone { get; private set; }
        //[Inject(Id = "AdventurerDiscardZone")] public ZoneDeckView AdventurerDiscardZone { get; private set; }
        //[Inject(Id = "AidZone")] public ZoneBasicView AidZone { get; private set; }
        //[Inject(Id = "SceneZone")] public ZoneBasicView SceneZone { get; private set; }
        //[Inject(Id = "SceneDeckZone")] public ZoneDeckView LocationsDeck { get; private set; }
        //[Inject(Id = "SceneDiscardZone")] public ZoneDeckView LocationsDiscard { get; private set; }
        //[Inject(Id = "PlaceZone")] public ZoneBasicView PlaceZone { get; private set; }
        //[Inject(Id = "GoalZone")] public ZoneBasicView GoalZone { get; private set; }
        //[Inject(Id = "PlotZone")] public ZoneBasicView PlotZone { get; private set; }
        //[Inject(Id = "FrontCameraZone")] public ZoneBasicView FrontCameraZone { get; private set; }
        //[Inject(Id = "OutGameZone")] public ZoneBasicView OutGameZone { get; private set; }

        /*******************************************************************/
        public ZoneView Get(Zone zone) => _allZones.Find(zoneView => zoneView.Zone == zone)
                                          ?? throw new KeyNotFoundException($"Zone {zone} not found");

        public ZoneView Get(string zoneName) => _allZones.Find(zoneView => zoneView.Zone.CodeName == zoneName)
                                                 ?? throw new KeyNotFoundException($"Zone {zoneName} not found");

        List<Zone> IZonesContainer.GetZones() => _allZones.Select(zoneView => zoneView.Zone).ToList();
    }
}
