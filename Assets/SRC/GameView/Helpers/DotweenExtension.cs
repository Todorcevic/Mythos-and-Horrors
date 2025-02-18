using DG.Tweening;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public static class DotweenExtension
    {
        public static Tween Wait(float delaySeconds)
        {
            return DOVirtual.DelayedCall(delaySeconds, () => { }, ignoreTimeScale: false);
        }

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

        public static Sequence DORecolocate(this Transform transform, float timeAnimation = ViewValues.FAST_TIME_ANIMATION, Ease ease = Ease.InCubic)
        {
            return DOTween.Sequence()
                .Join(transform.DOLocalMove(Vector3.zero, timeAnimation).SetEase(ease))
                .Join(transform.DOLocalRotate(Vector3.zero, timeAnimation))
                .Join(transform.DOScale(1f, timeAnimation));
        }

        public static Tween SetNotWaitable(this Tween tween)
        {
            return tween.SetId(ViewValues.NOT_WAITABLE_ANIMATION);
        }

        public static async Task WaitForAnimationsComplete()
        {
            while (DOTween.TotalPlayingTweens() > DOTween.TotalTweensById(ViewValues.NOT_WAITABLE_ANIMATION, playingOnly: true)) await Task.Yield();
        }

        public static async Task WaitForMoveToZoneComplete()
        {
            while (DOTween.TweensById(ViewValues.MOVE_ANIMATION, playingOnly: true)?.Count > 0) await Task.Yield();
        }
    }
}
