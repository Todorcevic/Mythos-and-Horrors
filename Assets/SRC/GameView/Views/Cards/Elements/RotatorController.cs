using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class RotatorController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rotator;

        /*******************************************************************/
        public Tween Rotate(bool rotate) => _rotator.DOLocalRotate(new Vector3(0, 0, rotate ? 180 : 0), ViewValues.DEFAULT_TIME_ANIMATION)
            .SetEase(Ease.OutCubic);

        public Tween Rotate360(float timeAnimation = ViewValues.SLOW_TIME_ANIMATION) =>
            _rotator.DOLocalRotate(new Vector3(0, 0, 360), timeAnimation, mode: RotateMode.FastBeyond360)
            .OnComplete(() => _rotator.localEulerAngles = Vector3.zero);

        public Tween RotateFake(float timeAnimation = ViewValues.DEFAULT_TIME_ANIMATION) => DOTween.Sequence()
            .Append(_rotator.DOLocalRotate(new Vector3(0, 0, 90), timeAnimation * 0.5f).SetEase(Ease.Linear))
            .Append(_rotator.DOLocalRotate(new Vector3(0, 0, 270), 0f))
            .Append(_rotator.DOLocalRotate(Vector3.zero, timeAnimation * 0.5f).SetEase(Ease.Linear));
    }
}
