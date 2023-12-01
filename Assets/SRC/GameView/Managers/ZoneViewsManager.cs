using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class ZoneViewsManager
    {
        private readonly List<ZoneView> _allZones = new();

        /*******************************************************************/
        public void Add(ZoneView zoneView) => _allZones.Add(zoneView);

        public ZoneView Get(Zone zone) => _allZones.First(zoneView => zoneView.Zone == zone);
    }
}
