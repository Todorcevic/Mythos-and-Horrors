using DG.Tweening;
using MythsAndHorrors.EditMode;
using UnityEngine;

namespace MythsAndHorrors.PlayMode
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
        public abstract Tween EnterCard(CardView cardView);

        public abstract Tween ExitCard(CardView cardView);

        public abstract Tween MouseEnter(CardView cardView);

        public abstract Tween MouseExit(CardView cardView);

        public abstract Tween MouseDrag(CardView cardView);
    }
}
