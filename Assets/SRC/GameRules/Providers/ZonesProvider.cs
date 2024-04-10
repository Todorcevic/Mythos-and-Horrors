using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ZonesProvider
    {
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        private readonly List<Zone> _zones = new();

        /*******************************************************************/
        public Zone Create(ZoneType zoneType)
        {
            Zone newZone = _diContainer.Instantiate<Zone>(new object[] { zoneType });
            _zones.Add(newZone);
            return newZone;
        }
        /*******************************************************************/

        public Zone GetZoneWithThisCard(Card card) => _zones.Find(zone => zone.Cards.Contains(card));

    }
}
