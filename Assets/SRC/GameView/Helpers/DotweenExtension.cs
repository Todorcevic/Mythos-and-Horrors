using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public static class DotweenExtension
    {
        public static Sequence DOFullMove(this Transform transform, Transform toMove, float timeAnimation = ViewValues.FAST_TIME_ANIMATION)
        {
            return DOTween.Sequence()
                .Join(transform.DOMove(toMove.position, timeAnimation))
                .Join(transform.DORotate(toMove.eulerAngles, timeAnimation))
                .Join(transform.DOScale(toMove.lossyScale, timeAnimation));
        }

        public static Sequence DOFullLocalMove(this Transform transform, Transform toMove, float timeAnimation = ViewValues.FAST_TIME_ANIMATION)
        {
            return DOTween.Sequence()
                .Join(transform.DOLocalMove(toMove.localPosition, timeAnimation))
                .Join(transform.DOLocalRotate(toMove.localEulerAngles, timeAnimation))
                .Join(transform.DOScale(toMove.lossyScale, timeAnimation));
        }

        public static Sequence DORecolocate(this Transform transform, float timeAnimation = ViewValues.FAST_TIME_ANIMATION)
        {
            return DOTween.Sequence()
                .Join(transform.DOLocalMove(Vector3.zero, timeAnimation).SetEase(Ease.InCubic))
                .Join(transform.DOLocalRotate(Vector3.zero, timeAnimation))
                .Join(transform.DOScale(1f, timeAnimation));
        }

        public static async Task WaitForAllTweensToComplete()
        {
            while (DOTween.TotalPlayingTweens() > 0) await Task.Yield();
        }
    }
}
