using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class RotatorController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _rotator;

        /*******************************************************************/
        public Tween Rotate(bool rotate) => _rotator.DOLocalRotate(new Vector3(0, 0, rotate ? 180 : 0), ViewValues.DEFAULT_TIME_ANIMATION)
            .SetEase(Ease.OutCubic);
    }
}
