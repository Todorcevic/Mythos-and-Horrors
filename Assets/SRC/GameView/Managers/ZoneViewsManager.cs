using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZoneViewsManager
    {
        private readonly List<ZoneView> _allZones = new();

        [Inject(Id = ZenjectBinding.BindId.SelectorZone)] public ZoneView SelectorZone { get; }
        [Inject(Id = ZenjectBinding.BindId.CenterShowZone)] public ZoneView CenterShowZone { get; }

        /*******************************************************************/
        public void Add(ZoneView zoneView) => _allZones.Add(zoneView);

        public ZoneView Get(Zone zone) => _allZones.First(zoneView => zoneView.Zone == zone);
    }
}
