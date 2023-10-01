using Tuesday.GameRules;
using Zenject;

namespace Tuesday.GameView
{
    public class ZonesManager
    {
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
        [Inject(Id = "OutGame")] public ZoneDeckView OutGame { get; private set; }

        /*******************************************************************/
        public ZoneView Get(ZoneType zone) => zone switch
        {
            ZoneType.Location => Location,
            ZoneType.LocationDeck => LocationsDeck,
            ZoneType.LocationDiscard => LocationsDiscard,
            ZoneType.AssetsDeck => AssetsDeck,
            ZoneType.AssetsDiscard => AssetsDiscard,
            ZoneType.Investigator => Investigator,
            ZoneType.Rewards => Rewards,
            ZoneType.FreeRow => FreeRow,
            ZoneType.PayRow => PayRow,
            ZoneType.OutGame => OutGame,
            _ => null,
        };
    }
}
