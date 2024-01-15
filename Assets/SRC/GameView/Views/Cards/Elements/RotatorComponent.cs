using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class RotatorComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rotator;

        /*******************************************************************/
        public Tween Rotate(bool rotate) => _rotator.DOLocalRotate(new Vector3(0, 0, rotate ? 180 : 0), ViewValues.DEFAULT_TIME_ANIMATION)
            .SetDelay(ViewValues.DEFAULT_TIME_ANIMATION * 0.5f).SetEase(Ease.InOutCubic);
    }
}
