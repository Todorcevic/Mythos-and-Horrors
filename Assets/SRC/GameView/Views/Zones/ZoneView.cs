using DG.Tweening;
using MythsAndHorrors.GameRules;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public abstract class ZoneView : MonoBehaviour
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
    }
}
