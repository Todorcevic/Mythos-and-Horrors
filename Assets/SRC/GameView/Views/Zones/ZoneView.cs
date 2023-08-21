using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameView
{
    public abstract class ZoneView : MonoBehaviour
    {
        protected List<CardView> AllCards => GetComponentsInChildren<CardView>().ToList();
        protected float YOffSet => AllCards.Count * ViewValues.CARD_THICKNESS;

        /*******************************************************************/
        public abstract Tween MoveCard(CardView card);
    }
}
