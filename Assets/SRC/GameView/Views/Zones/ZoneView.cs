using DG.Tweening;
using MythsAndHorrors.GameRules;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public abstract class ZoneView : MonoBehaviour, IZoneBehaviour
    {
        public Zone Zone { get; private set; }

        /*******************************************************************/
        private void Awake()
        {
            Zone = new Zone(name);
        }

        /*******************************************************************/
        public abstract Tween MoveCard(CardView cardView);

        public abstract Tween RemoveCard(CardView cardView);

        public abstract void MouseEnter(CardView cardView);

        public abstract void MouseExit(CardView cardView);

        public abstract void MouseDrag(CardView cardView);
    }
}
