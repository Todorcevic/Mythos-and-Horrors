using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class SwapInvestigatorComponent : MonoBehaviour
    {
        private AreaInvestigatorView _currentAreaInvestigator;
        [Inject] private readonly AreaInvestigatorViewsManager _areaInvestigatorViewsManager;
        [Inject] private readonly AudioComponent _audioComponent;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _playPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rightPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _leftPosition;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _optimizationPosition;
        [SerializeField, Required, AssetsOnly] private AudioClip _swapAudio;


        /*******************************************************************/
        public Tween Select(Investigator investigator)
        {
            AreaInvestigatorView areaInvestigatorView = _areaInvestigatorViewsManager.Get(investigator);
            (Transform positionToExit, Transform positionToEnter) = GetSidePosition(investigator);
            return DOTween.Sequence().OnStart(() => { areaInvestigatorView.transform.position = positionToEnter.position; _audioComponent.PlayAudio(_swapAudio); })
                .Append(areaInvestigatorView.transform.DOFullMove(_playPosition, ViewValues.MID_TIME_ANIMATION).SetEase(Ease.InOutQuad))
                .Join(_currentAreaInvestigator == null ? DOTween.Sequence() :
                    _currentAreaInvestigator.transform.DOFullMove(positionToExit, ViewValues.MID_TIME_ANIMATION).SetEase(Ease.InOutQuad))
                    .OnComplete(() => _currentAreaInvestigator = areaInvestigatorView);
        }

        private (Transform, Transform) GetSidePosition(Investigator investigator) =>
            investigator.Position > _currentAreaInvestigator?.Investigator.Position ?
            (_leftPosition, _rightPosition) : (_rightPosition, _leftPosition);
    }
}
