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

        public abstract Tween MouseEnter(CardView cardView);

        public abstract Tween MouseExit(CardView cardView);

        public abstract Tween MouseDrag(CardView cardView);
    }
}
