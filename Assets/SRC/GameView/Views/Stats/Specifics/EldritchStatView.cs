using DG.Tweening;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class EldritchStatView : StatView
    {
        public override Tween UpdateValue()
        {
            return DOTween.Sequence()
                  .Append(_value.transform.DOScale(Vector3.zero, ViewValues.FAST_TIME_ANIMATION).SetEase(Ease.InCubic))
                  .InsertCallback(ViewValues.FAST_TIME_ANIMATION, () => _value.text = Stat.Value.ToString())
                  .Append(_value.transform.DOScale(Vector3.one, ViewValues.FAST_TIME_ANIMATION * 0.75f).SetEase(Ease.OutBack, 3f));
        }
    }
}
