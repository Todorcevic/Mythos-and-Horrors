using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZoneViewsManager
    {
        private readonly List<ZoneView> _allZones = new();

        [Inject(Id = ZenjectBinding.BindId.SelectorZone)] public ZoneView SelectorZone { get; }
        [Inject(Id = ZenjectBinding.BindId.OutZone)] public ZoneView OutZone { get; }
        [Inject(Id = ZenjectBinding.BindId.CenterShow)] public Transform CenterShow { get; }

        /*******************************************************************/
        public void Add(ZoneView zoneView) => _allZones.Add(zoneView);

        public ZoneView Get(Zone zone) => _allZones.First(zoneView => zoneView.Zone == zone);
    }
}
