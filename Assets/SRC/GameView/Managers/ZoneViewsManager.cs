using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZoneViewsManager
    {
        private readonly List<ZoneView> _allZones = new();

        [Inject(Id = ZenjectBinding.BindId.SelectorZone)] public ShowSelectorZoneView SelectorZone { get; }
        [Inject(Id = ZenjectBinding.BindId.CenterShowZone)] public ZoneView CenterShowZone { get; }
        [Inject(Id = ZenjectBinding.BindId.OutZone)] public ZoneView OutZone { get; }

        /*******************************************************************/
        public void Add(ZoneView zoneView)
        {
            if (_allZones.Select(z => z.Zone).Contains(zoneView.Zone)) throw new Exception($"Zone {zoneView.Zone} already added");
            _allZones.Add(zoneView);
        }

        public ZoneView Get(Zone zone) => _allZones.First(zoneView => zoneView.Zone == zone);
    }
}
