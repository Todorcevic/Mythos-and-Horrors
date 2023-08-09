using System.Collections.Generic;
using Zenject;

namespace GameView
{
    public class ZonesManager
    {
        [Inject] private readonly List<ZoneView> _allZones;

        [Inject(Id = "Investigator")] public ZoneView Investigator { get; private set; }
        [Inject(Id = "LocationsDeck")] public ZoneView LocationsDeck { get; private set; }
        [Inject(Id = "LocationsDiscard")] public ZoneView LocationsDiscard { get; private set; }
        [Inject(Id = "Location")] public ZoneView Location { get; private set; }
        [Inject(Id = "Rewards")] public ZoneView Rewards { get; private set; }
        [Inject(Id = "FreeRow")] public ZoneView FreeRow { get; private set; }
        [Inject(Id = "PayRow")] public ZoneView PayRow { get; private set; }
        [Inject(Id = "AssetsDeck")] public ZoneView AssetsDeck { get; private set; }
        [Inject(Id = "AssetsDiscard")] public ZoneView AssetsDiscard { get; private set; }
        [Inject(Id = "FrontCamera")] public ZoneView FrontCamera { get; private set; }

        /*******************************************************************/
    }
}
