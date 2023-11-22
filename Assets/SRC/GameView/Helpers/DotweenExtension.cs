using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public static class DotweenExtension
    {
        public static Sequence DOFullMove(this Transform transform, Transform toMove, float velocity = ViewValues.FAST_TIME_ANIMATION)
        {
            return DOTween.Sequence()
                .Join(transform.DOMove(toMove.position, velocity))
                .Join(transform.DORotate(toMove.eulerAngles, velocity))
                .Join(transform.DOScale(toMove.lossyScale, velocity));
        }

        public static Sequence DOFullLocalMove(this Transform transform, Transform toMove, float velocity = ViewValues.FAST_TIME_ANIMATION)
        {
            return DOTween.Sequence()
                .Join(transform.DOLocalMove(toMove.localPosition, velocity))
                .Join(transform.DOLocalRotate(toMove.localEulerAngles, velocity))
                .Join(transform.DOScale(toMove.lossyScale, velocity));
        }
    }
}
