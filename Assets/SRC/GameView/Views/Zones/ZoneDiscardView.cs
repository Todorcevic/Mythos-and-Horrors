using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneDiscardView : ZoneView
    {
        private const float OFF_SET_EXPAND_Y = -1.5f;
        [SerializeField, Required] protected Transform _movePosition;
        [SerializeField, Required] protected Transform _hoverPosition;
        private Sequence current;
        private readonly List<CardView> _allCards = new();

        private float YOffSet => _allCards.Count * ViewValues.CARD_THICKNESS;
        private int LastIndex => _allCards.Count - 1;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView)
        {
            _allCards.Add(cardView);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            return cardView.transform.DOFullMove(_movePosition, 0)
                .OnComplete(() => cardView.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView cardView)
        {
            _allCards.Remove(cardView);
            return DOTween.Sequence();
        }

        public override void MouseDrag(CardView cardView) { }

        public override void MouseEnter(CardView cardView)
        {
            current?.Kill(); //Avoid jittering
            current = transform.DOFullMove(_hoverPosition);
            current.AppendInterval(0);

            for (int j = 0; j <= LastIndex; j++)
            {
                CardView cardV = _allCards[LastIndex - j];
                current.Join(cardV.transform.DOLocalMoveX(OFF_SET_EXPAND_Y * j, ViewValues.FAST_TIME_ANIMATION));
            }
        }

        public override void MouseExit(CardView cardView)
        {
            current?.Kill(); //Avoid jittering
            current = transform.DOFullMove(transform.parent);

            for (int j = 0; j <= LastIndex; j++)
            {
                CardView cardV = _allCards[LastIndex - j];
                current.Join(cardV.transform.DOLocalMoveX(0, ViewValues.FAST_TIME_ANIMATION));
            }
        }
    }
}
