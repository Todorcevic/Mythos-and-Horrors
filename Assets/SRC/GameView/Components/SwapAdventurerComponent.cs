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

            return DOTween.Sequence().PrependCallback(Initialize)
                .Join(_currentAreaAdventurer.transform.DOFullMove(positionToMove)
                .Join(areaAdventurerView.transform.DOFullMove(_playPosition))
                .AppendCallback(Finish));

            void Finish()
            {
                _currentAreaAdventurer.gameObject.SetActive(false);
                _currentAreaAdventurer.transform.SetParent(positionToMove);
                _currentAreaAdventurer = areaAdventurerView;
            }

            void Initialize()
            {
                areaAdventurerView.gameObject.SetActive(true);
                areaAdventurerView.transform.SetParent(_playPosition, worldPositionStays: true);
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
