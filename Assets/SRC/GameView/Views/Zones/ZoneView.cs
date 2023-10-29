using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public abstract class ZoneView : MonoBehaviour
    {
        public Zone Zone { get; private set; }
        protected List<CardView> AllCards => GetComponentsInChildren<CardView>().ToList();
        protected float YOffSet => AllCards.Count * ViewValues.CARD_THICKNESS;

        /*******************************************************************/
        private void Awake()
        {
            Zone = new Zone(name);
        }

        /*******************************************************************/
        public abstract Tween MoveCard(CardView card);

        public abstract Tween RemoveCard(CardView card);
    }
}
