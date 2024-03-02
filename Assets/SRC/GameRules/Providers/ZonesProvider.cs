using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ZonesProvider
    {
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        private readonly List<Zone> _zones = new();

        /*******************************************************************/
        public Zone Create()
        {
            Zone newZone = _diContainer.Instantiate<Zone>();
            _zones.Add(newZone);
            return newZone;
        }
        /*******************************************************************/

        public bool IsSceneZone(Zone zone) => _chaptersProvider.CurrentScene.HasThisZone(zone);

        public Zone GetZoneWithThisCard(Card card) => _zones.Find(zone => zone.Cards.Contains(card));

    }
}
