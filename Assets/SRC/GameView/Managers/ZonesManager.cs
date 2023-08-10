using System.Collections.Generic;
using Zenject;

namespace GameView
{
    public class ZonesManager
    {
        [Inject] private readonly List<ZoneBase> _allZones;

        [Inject(Id = "Investigator")] public ZoneBasicView Investigator { get; private set; }
        [Inject(Id = "LocationsDeck")] public ZoneDeckView LocationsDeck { get; private set; }
        [Inject(Id = "LocationsDiscard")] public ZoneDeckView LocationsDiscard { get; private set; }
        [Inject(Id = "Location")] public ZoneBasicView Location { get; private set; }
        [Inject(Id = "Rewards")] public ZoneRowView Rewards { get; private set; }
        [Inject(Id = "FreeRow")] public ZoneRowView FreeRow { get; private set; }
        [Inject(Id = "PayRow")] public ZoneRowView PayRow { get; private set; }
        [Inject(Id = "AssetsDeck")] public ZoneDeckView AssetsDeck { get; private set; }
        [Inject(Id = "AssetsDiscard")] public ZoneDeckView AssetsDiscard { get; private set; }
        [Inject(Id = "FrontCamera")] public ZoneBasicView FrontCamera { get; private set; }

        /*******************************************************************/
    }
}
