﻿using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class AreaPlacesView : MonoBehaviour
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private List<ZoneView> _rowPlaces0;
        [SerializeField, Required, ChildGameObjectsOnly] private List<ZoneView> _rowPlaces1;
        [SerializeField, Required, ChildGameObjectsOnly] private List<ZoneView> _rowPlaces2;

        /*******************************************************************/
        public void Init()
        {
            _rowPlaces0.ForEach(zone => zone.Init(_chaptersProvider.CurrentScene.GetPlaceZone(0, _rowPlaces0.IndexOf(zone))));
            _rowPlaces1.ForEach(zone => zone.Init(_chaptersProvider.CurrentScene.GetPlaceZone(1, _rowPlaces1.IndexOf(zone))));
            _rowPlaces2.ForEach(zone => zone.Init(_chaptersProvider.CurrentScene.GetPlaceZone(2, _rowPlaces2.IndexOf(zone))));
        }
    }
}
