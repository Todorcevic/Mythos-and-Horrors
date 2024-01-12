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
        [SerializeField, Required, ChildGameObjectsOnly] private AreaInvestigatorView _currentAreaInvestigator;

        /*******************************************************************/
        public Tween Select(Investigator investigator)
        {
            AreaInvestigatorView areaInvestigatorView = _areaInvestigatorViewsManager.Get(investigator);
            (Transform positionToExit, Transform positionToEnter) = GetSidePosition(investigator);

            return DOTween.Sequence()
                .Join(areaInvestigatorView.transform.DOFullMoveSlow(_playPosition).SetEase(Ease.InOutCubic)
                .OnStart(() => areaInvestigatorView.transform.position = positionToEnter.position))
                .Join(_currentAreaInvestigator.transform.DOFullMoveSlow(positionToExit).SetEase(Ease.InOutCubic)
                .OnComplete(() => _currentAreaInvestigator.transform.position *= 4f))
                .OnComplete(Finish);

            void Finish() => _currentAreaInvestigator = areaInvestigatorView;

        }

        private (Transform, Transform) GetSidePosition(Investigator investigator) =>
            _investigatorsProvider.GetInvestigatorPosition(investigator) >
            _investigatorsProvider.GetInvestigatorPosition(_currentAreaInvestigator.Investigator) ?
            (_leftPosition, _rightPosition) : (_rightPosition, _leftPosition);
    }
}
