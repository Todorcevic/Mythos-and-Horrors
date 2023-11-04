using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneView : MonoBehaviour, IZoneBehaviour
    {
        [SerializeField, Required] protected Transform _movePosition;
        [SerializeField, Required] protected Transform _hoverPosition;
        [SerializeField, Required] protected Transform _showPosition;

        public Zone Zone { get; private set; }

        /*******************************************************************/
        private void Awake()
        {
            Zone = new Zone(name);
        }

        /*******************************************************************/
        public virtual Tween MoveCard(CardView cardView)
        {
            return cardView.transform.DOFullMove(_movePosition)
                .OnComplete(() => cardView.SetCurrentZoneView(this));
        }

        public virtual Tween RemoveCard(CardView cardView)
        {
            return DOTween.Sequence();
        }

        public virtual void MouseEnter(CardView cardView)
        {
            cardView.transform.DOFullMove(_hoverPosition);
        }

        public virtual void MouseExit(CardView cardView)
        {
            cardView.transform.DOFullMove(_movePosition);
        }

        public virtual void MouseDrag(CardView cardView) { }
    }
}
