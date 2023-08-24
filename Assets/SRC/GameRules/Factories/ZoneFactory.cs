using System;
using Zenject;

namespace GameRules
{
    public class ZoneFactory
    {
        [Inject] private readonly ZoneRepository _zoneRepository;

        /*******************************************************************/
        public void CreateZones()
        {
            foreach (ZoneType zoneType in Enum.GetValues(typeof(ZoneType)))
            {
                _zoneRepository.AddZone(new Zone(zoneType));
            }
        }
    }
}
