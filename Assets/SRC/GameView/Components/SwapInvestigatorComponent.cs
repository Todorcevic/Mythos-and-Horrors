using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SwapInvestigatorComponent : MonoBehaviour
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _playPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rightPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _leftPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _optimizationPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private AreaInvestigatorView _currentAreaInvestigator;

        /*******************************************************************/
        public Tween Select(Investigator investigator)
        {
            AreaInvestigatorView areaInvestigatorView = _areaInvestigatorViewsManager.Get(investigator);
            (Transform positionToExit, Transform positionToEnter) = GetSidePosition(investigator);
            Sequence swapSequence = DOTween.Sequence().OnStart(() => areaInvestigatorView.transform.position = positionToEnter.position)
                .Append(areaInvestigatorView.transform.DOFullMove(_playPosition, ViewValues.SLOW_TIME_ANIMATION).SetEase(Ease.InOutCubic))
                .Join(_currentAreaInvestigator.transform.DOFullMove(positionToExit, ViewValues.SLOW_TIME_ANIMATION).SetEase(Ease.InOutCubic))
                .OnComplete(() =>
                {
                    _currentAreaInvestigator.transform.position = _optimizationPosition.position;
                    _currentAreaInvestigator = areaInvestigatorView;
                });
            return swapSequence;
        }

        private (Transform, Transform) GetSidePosition(Investigator investigator) =>
            _investigatorsProvider.GetInvestigatorPosition(investigator) >
            _investigatorsProvider.GetInvestigatorPosition(_currentAreaInvestigator.Investigator) ?
            (_leftPosition, _rightPosition) : (_rightPosition, _leftPosition);
    }
}
