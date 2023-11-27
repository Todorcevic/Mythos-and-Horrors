using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ZoneViewsManager
    {
        [Inject] private readonly List<ZoneView> _allZones;
        [Inject] private readonly InitializeAreaUseCase _loadAreaUseCase;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            _loadAreaUseCase.Execute();
        }

        /*******************************************************************/
        public ZoneView Get(Zone zone) => _allZones.First(zoneView => zoneView.Zone == zone);

        public ZoneView Get(string zoneName) => _allZones.First(zoneView => zoneView.name == zoneName);
    }
}
