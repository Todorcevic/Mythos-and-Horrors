using DG.Tweening;
using MythsAndHorrors.GameRules;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public abstract class ZoneView : MonoBehaviour
    {
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;

        public Zone Zone { get; private set; }

        /*******************************************************************/
        public void Init(Zone zone)
        {
            Zone = zone;
            _zoneViewsManager.Add(this);
        }

        /*******************************************************************/
        public abstract Tween EnterZone(CardView cardView);

        public abstract Tween ExitZone(CardView cardView);

        public abstract Tween MouseEnter(CardView cardView);

        public abstract Tween MouseExit(CardView cardView);

        public abstract Tween MouseDrag(CardView cardView);

        public virtual Tween Shuffle() => DOTween.Sequence();
    }
}
