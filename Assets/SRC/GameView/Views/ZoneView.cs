using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameView
{
    public class ZoneView : MonoBehaviour
    {
        private List<CardView> AllCards => GetComponentsInChildren<CardView>().ToList();
        private float YOffSet => AllCards.Count * ViewValues.CARD_THICKNESS;

        /*******************************************************************/
        public Tween MoveCard(CardView card)
        {
            return DOTween.Sequence()
                .Join(card.transform.DOMove(transform.position + new Vector3(0, YOffSet, 0), ViewValues.SLOW_TIME_ANIMATION))
                .Join(card.transform.DORotate(transform.eulerAngles, ViewValues.SLOW_TIME_ANIMATION))
                .Join(card.transform.DOScale(transform.localScale, ViewValues.SLOW_TIME_ANIMATION))
                .OnComplete(() => card.transform.SetParent(transform));
        }
    }
}
