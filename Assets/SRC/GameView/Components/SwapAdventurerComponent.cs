using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SwapAdventurerComponent : MonoBehaviour
    {
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly List<AreaAdventurerView> _allAreas;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _playPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rightPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _leftPosition;
        private AreaAdventurerView _currentAreaAdventurer;

        /*******************************************************************/
        public Tween Select(Adventurer adventurer)
        {
            AreaAdventurerView areaAdventurerView = Get(adventurer);
            Transform positionToMove = GetSidePosition(adventurer);

            return DOTween.Sequence().Join(_currentAreaAdventurer.transform.DOFullMove(_playPosition, ViewValues.FAST_TIME_ANIMATION)
                .OnComplete(() => _currentAreaAdventurer.transform.SetParent(_playPosition)))
                .Join(areaAdventurerView.transform.DOFullMove(_playPosition, ViewValues.FAST_TIME_ANIMATION))
                .OnComplete(() => areaAdventurerView.transform.SetParent(_playPosition))
                .OnComplete(() => _currentAreaAdventurer = areaAdventurerView);
        }

        private AreaAdventurerView Get(Adventurer adventurer) => _allAreas.First(areaView => areaView.Adventurer == adventurer);

        private Transform GetSidePosition(Adventurer adventurer) =>
            _adventurersProvider.GetAdventurerPosition(adventurer) >
            _adventurersProvider.GetAdventurerPosition(_currentAreaAdventurer.Adventurer) ?
            _leftPosition :
            _rightPosition;


    }
}
