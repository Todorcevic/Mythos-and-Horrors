using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;
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

        public static Sequence DOFullLocalDefaultMove(this Transform transform, Transform toMove)
        {
            return DOTween.Sequence()
                .Join(transform.DOLocalMove(toMove.localPosition, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOLocalRotate(toMove.localEulerAngles, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOScale(toMove.lossyScale, ViewValues.DEFAULT_TIME_ANIMATION));
        }

        public static Sequence DOFullLocalSlowMove(this Transform transform, Transform toMove)
        {
            return DOTween.Sequence()
                .Join(transform.DOLocalMove(toMove.localPosition, ViewValues.SLOW_TIME_ANIMATION))
                .Join(transform.DOLocalRotate(toMove.localEulerAngles, ViewValues.SLOW_TIME_ANIMATION))
                .Join(transform.DOScale(toMove.lossyScale, ViewValues.SLOW_TIME_ANIMATION));
        }

        public static Sequence DOFullMoveSlow(this Transform transform, Transform toMove)
        {
            return DOTween.Sequence()
                .Join(transform.DOMove(toMove.position, ViewValues.SLOW_TIME_ANIMATION))
                .Join(transform.DORotate(toMove.eulerAngles, ViewValues.SLOW_TIME_ANIMATION))
                .Join(transform.DOScale(toMove.lossyScale, ViewValues.SLOW_TIME_ANIMATION));
        }

        public static Sequence DOFullMoveDefault(this Transform transform, Transform toMove)
        {
            return DOTween.Sequence()
                .Join(transform.DOMove(toMove.position, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DORotate(toMove.eulerAngles, ViewValues.DEFAULT_TIME_ANIMATION))
                .Join(transform.DOScale(toMove.lossyScale, ViewValues.DEFAULT_TIME_ANIMATION));
        }

        public static async Task WaitForAllTweensToComplete()
        {
            while (DOTween.TotalPlayingTweens() > 0) await Task.Yield();
        }
    }
}
