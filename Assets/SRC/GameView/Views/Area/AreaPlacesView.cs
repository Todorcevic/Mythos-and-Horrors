using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class AreaPlacesView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<ZoneView> _rowPlaces0;
        [SerializeField, Required, ChildGameObjectsOnly] private List<ZoneView> _rowPlaces1;
        [SerializeField, Required, ChildGameObjectsOnly] private List<ZoneView> _rowPlaces2;

        /*******************************************************************/
        public void Init(ZonesProvider zonesProvider)
        {
            _rowPlaces0.ForEach(zone => zone.Init(zonesProvider.PlaceZone[0, _rowPlaces0.IndexOf(zone)]));
            _rowPlaces1.ForEach(zone => zone.Init(zonesProvider.PlaceZone[1, _rowPlaces1.IndexOf(zone)]));
            _rowPlaces2.ForEach(zone => zone.Init(zonesProvider.PlaceZone[2, _rowPlaces2.IndexOf(zone)]));
        }
    }
}
