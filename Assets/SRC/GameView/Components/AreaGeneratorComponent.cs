using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AreaGeneratorComponent : MonoBehaviour
    {
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly SwapAdventurerComponent _swapAdventurerComponent;
        [SerializeField, Required, AssetsOnly] private AreaAdventurerView _areaAdventurerPrefab;
        [SerializeField, Required] private AreaSceneView _areaScenePrefab;
        [SerializeField, Required] private AreaPlacesView _areaPlacesPrefab;

        /*******************************************************************/
        public void BuildAdventurerAreas()
        {
            List<AreaAdventurerView> areaAdventurerViews = new();
            _adventurersProvider.GetAllAdventurers()
                .ForEach(adventurer => areaAdventurerViews.Add(_diContainer.InstantiatePrefabForComponent<AreaAdventurerView>(_areaAdventurerPrefab, transform, new object[] { adventurer })));

            _swapAdventurerComponent.Init(areaAdventurerViews);
        }

        public void BuildSceneArea() => _areaScenePrefab.Init(_zonesProvider);

        public void BuildPlacesArea() => _areaPlacesPrefab.Init(_zonesProvider);
    }
}
