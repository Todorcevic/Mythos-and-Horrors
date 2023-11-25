using MythsAndHorrors.GameRules;
using Sirenix.Utilities;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class LoadAreaUseCase
    {
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly List<AreaAdventurerView> _adventurerAreas;
        [Inject] private readonly AreaSceneView _sceneArea;
        [Inject] private readonly AreaPlacesView _placesArea;

        /*******************************************************************/
        public void BuildAdventurerAreas() =>
            _adventurersProvider.GetAllAdventurers().ForEach(adventurer => _adventurerAreas.Find(area => area.IsFree).Init(adventurer));

        public void BuildSceneArea() => _sceneArea.Init(_zonesProvider);

        public void BuildPlacesArea() => _placesArea.Init(_zonesProvider);
    }
}
