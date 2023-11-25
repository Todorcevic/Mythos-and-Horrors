using DG.Tweening;
using MythsAndHorrors.GameRules;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public abstract class ZoneView : MonoBehaviour
    {
        public Zone Zone { get; private set; }

        /*******************************************************************/
        public void Init(Zone zone)
        {
            Zone = zone;
        }

        /*******************************************************************/
        public abstract Tween EnterCard(CardView cardView, float timeAnimation = ViewValues.FAST_TIME_ANIMATION);

        public abstract Tween ExitCard(CardView cardView, float timeAnimation = ViewValues.FAST_TIME_ANIMATION);

        public abstract Tween MouseEnter(CardView cardView);

        public abstract Tween MouseExit(CardView cardView);

        public abstract Tween MouseDrag(CardView cardView);
    }
}
