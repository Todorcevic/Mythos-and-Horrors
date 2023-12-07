using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SwapAdventurerComponent : MonoBehaviour
    {
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [SerializeField, Required, ChildGameObjectsOnly] public List<AreaAdventurerView> _allAdventurerAreas;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _playPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rightPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _leftPosition;
        private AreaAdventurerView _currentAreaAdventurer;

        public Adventurer AdventurerSelected => _currentAreaAdventurer.Adventurer;

        /*******************************************************************/
        public void Init()
        {
            _adventurersProvider.AllAdventurers.ForEach(adventurer => _allAdventurerAreas.First(area => area.IsFree).Init(adventurer));
            _currentAreaAdventurer = _allAdventurerAreas.First();
        }

        /*******************************************************************/
        public Tween Select(Adventurer adventurer)
        {
            AreaAdventurerView areaAdventurerView = Get(adventurer);
            Transform positionToMove = GetSidePosition(adventurer);

            return DOTween.Sequence()
                .Join(areaAdventurerView.transform.DOFullMove(_playPosition).OnStart(() => areaAdventurerView.transform.position *= 0.25f))
                .Join(_currentAreaAdventurer.transform.DOFullMove(positionToMove).OnComplete(() => _currentAreaAdventurer.transform.position *= 4f))
                .OnComplete(Finish);

            void Finish()
            {
                _currentAreaAdventurer = areaAdventurerView;
            }
        }

        private AreaAdventurerView Get(Adventurer adventurer) => _allAdventurerAreas.First(areaView => areaView.Adventurer == adventurer);

        private Transform GetSidePosition(Adventurer adventurer) =>
            _adventurersProvider.GetAdventurerPosition(adventurer) >
            _adventurersProvider.GetAdventurerPosition(_currentAreaAdventurer.Adventurer) ?
            _leftPosition :
            _rightPosition;
    }
}
