using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneRowView : ZoneView
    {
        /*******************************************************************/
        public override Tween MoveCard(CardView card)
        {
            return DOTween.Sequence()
                .Join(card.transform.DOMove(transform.position + new Vector3(0, YOffSet, 0), ViewValues.SLOW_TIME_ANIMATION))
                .Join(card.transform.DORotate(transform.eulerAngles, ViewValues.SLOW_TIME_ANIMATION))
                .Join(card.transform.DOScale(transform.localScale, ViewValues.SLOW_TIME_ANIMATION))
                .OnComplete(() => card.transform.SetParent(transform));
        }
    }
}
