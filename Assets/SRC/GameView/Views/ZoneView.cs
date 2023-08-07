using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameView
{
    public class ZoneView : MonoBehaviour
    {
        private List<CardView> AllCards => GetComponentsInChildren<CardView>().ToList();

        /*******************************************************************/
        public void MoveCard(CardView card)
        {
            card.transform.SetParent(transform);
            card.transform.DOMove(transform.position, ViewValues.SLOW_TIME_ANIMATION);
            card.transform.DORotate(transform.eulerAngles, ViewValues.SLOW_TIME_ANIMATION);
            card.transform.DOScale(transform.localScale, ViewValues.SLOW_TIME_ANIMATION);
        }
    }
}
