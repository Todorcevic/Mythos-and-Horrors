using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ZonesProvider
    {
        [Inject] private readonly OwnersProvider _ownersProvider;

        public Zone OutZone { get; private set; }
        private IEnumerable<Zone> Zones => _ownersProvider.AllOwners.SelectMany(owner => owner.FullZones).Append(OutZone);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            OutZone = new(ZoneType.Out);
        }

        /*******************************************************************/
        public Zone GetZoneWithThisCard(Card card) => Zones.First(zone => zone.Cards.Contains(card));
    }
}
