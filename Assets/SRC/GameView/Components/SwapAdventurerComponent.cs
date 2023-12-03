using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SwapAdventurerComponent : MonoBehaviour
    {
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly IOActivatorComponent _iOActivatorComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private List<AreaAdventurerView> _allAdventurerAreas;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _playPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rightPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _leftPosition;
        private AreaAdventurerView _currentAreaAdventurer;

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
                .PrependCallback(_iOActivatorComponent.DesactivateSensor)
                .Join(areaAdventurerView.transform.DOFullMove(_playPosition).OnStart(() => _currentAreaAdventurer.transform.position *= 0.25f))
                .Join(_currentAreaAdventurer.transform.DOFullMove(positionToMove).OnComplete(() => _currentAreaAdventurer.transform.position *= 4f))
                .AppendCallback(Finish);

            void Finish()
            {
                areaAdventurerView.transform.SetParent(_playPosition);
                _currentAreaAdventurer.transform.SetParent(positionToMove);
                _currentAreaAdventurer = areaAdventurerView;
                _iOActivatorComponent.ActivateSensor();
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
