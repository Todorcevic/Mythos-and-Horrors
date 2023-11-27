using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public static class DotweenExtension
    {
        public static Sequence DOFullMove(this Transform transform, Transform toMove)
        {
            return DOTween.Sequence()
                .Join(transform.DOMove(toMove.position, ViewValues.FAST_TIME_ANIMATION))
                .Join(transform.DORotate(toMove.eulerAngles, ViewValues.FAST_TIME_ANIMATION))
                .Join(transform.DOScale(toMove.lossyScale, ViewValues.FAST_TIME_ANIMATION));
        }

        public static Sequence DOFullLocalMove(this Transform transform, Transform toMove)
        {
            return DOTween.Sequence()
                .Join(transform.DOLocalMove(toMove.localPosition, ViewValues.FAST_TIME_ANIMATION))
                .Join(transform.DOLocalRotate(toMove.localEulerAngles, ViewValues.FAST_TIME_ANIMATION))
                .Join(transform.DOScale(toMove.lossyScale, ViewValues.FAST_TIME_ANIMATION));
        }
    }
}
