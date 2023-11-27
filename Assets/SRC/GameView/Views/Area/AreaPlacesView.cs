using MythsAndHorrors.EditMode;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.PlayMode
{
    public class AreaPlacesView : MonoBehaviour
    {
        [Inject] private readonly ZonesProvider _zonesProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private List<ZoneView> _rowPlaces0;
        [SerializeField, Required, ChildGameObjectsOnly] private List<ZoneView> _rowPlaces1;
        [SerializeField, Required, ChildGameObjectsOnly] private List<ZoneView> _rowPlaces2;

        /*******************************************************************/
        public void Init()
        {
            _rowPlaces0.ForEach(zone => zone.Init(_zonesProvider.PlaceZone[0, _rowPlaces0.IndexOf(zone)]));
            _rowPlaces1.ForEach(zone => zone.Init(_zonesProvider.PlaceZone[1, _rowPlaces1.IndexOf(zone)]));
            _rowPlaces2.ForEach(zone => zone.Init(_zonesProvider.PlaceZone[2, _rowPlaces2.IndexOf(zone)]));
        }
    }
}
