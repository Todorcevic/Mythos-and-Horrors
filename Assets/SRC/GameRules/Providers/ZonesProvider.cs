using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ZonesProvider
    {
        [Inject] private readonly DiContainer _diContainer;
        private readonly List<Zone> _zones = new();

        public Zone OutZone { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            OutZone = Create(ZoneType.Out);
        }

        /*******************************************************************/
        public Zone Create(ZoneType zoneType)
        {
            Zone newZone = _diContainer.Instantiate<Zone>(new object[] { zoneType });
            _zones.Add(newZone);
            return newZone;
        }
        /*******************************************************************/

        public Zone GetZoneWithThisCard(Card card) => _zones.First(zone => zone.Cards.Contains(card));
    }
}
