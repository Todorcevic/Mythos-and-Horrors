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
    public class SwapInvestigatorComponent : MonoBehaviour
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [SerializeField, Required, ChildGameObjectsOnly] public List<AreaInvestigatorView> _allInvestigatorAreas;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _playPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rightPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _leftPosition;
        private AreaInvestigatorView _currentAreaInvestigator;

        public Investigator InvestigatorSelected => _currentAreaInvestigator.Investigator;

        /*******************************************************************/
        public void Init()
        {
            _investigatorsProvider.AllInvestigators.ForEach(investigator => _allInvestigatorAreas.First(area => area.IsFree).Init(investigator));
            _currentAreaInvestigator = _allInvestigatorAreas.First();
        }

        /*******************************************************************/
        public Tween Select(Investigator investigator)
        {
            AreaInvestigatorView areaInvestigatorView = Get(investigator);
            (Transform positionToExit, Transform positionToEnter) = GetSidePosition(investigator);

            return DOTween.Sequence()
                .Join(areaInvestigatorView.transform.DOFullMoveSlow(_playPosition).SetEase(Ease.InOutCubic)
                .OnStart(() => areaInvestigatorView.transform.position = positionToEnter.position))
                .Join(_currentAreaInvestigator.transform.DOFullMoveSlow(positionToExit).SetEase(Ease.InOutCubic)
                .OnComplete(() => _currentAreaInvestigator.transform.position *= 4f))
                .OnComplete(Finish);

            void Finish() => _currentAreaInvestigator = areaInvestigatorView;

        }

        public AreaInvestigatorView Get(Investigator investigator) => _allInvestigatorAreas.First(areaView => areaView.Investigator == investigator);

        private (Transform, Transform) GetSidePosition(Investigator investigator) =>
            _investigatorsProvider.GetInvestigatorPosition(investigator) >
            _investigatorsProvider.GetInvestigatorPosition(_currentAreaInvestigator.Investigator) ?
            (_leftPosition, _rightPosition) : (_rightPosition, _leftPosition);
    }
}
