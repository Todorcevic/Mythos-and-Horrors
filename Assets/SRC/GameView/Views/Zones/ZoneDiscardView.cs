using Codice.Client.BaseCommands;
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
        private bool isStandUp;
        private Sequence currentSequence;
        private readonly List<CardView> _allCards = new();

        private float YOffSet => _allCards.Count * ViewValues.CARD_THICKNESS;
        private int LastIndex => _allCards.Count - 1;

        /*******************************************************************/
        public override Tween MoveCard(CardView cardView)
        {
            _allCards.Add(cardView);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            cardView.SetCurrentZoneView(this);
            return cardView.transform.DOFullMove(_movePosition, 0);
        }

        public override Tween RemoveCard(CardView cardView)
        {
            _allCards.Remove(cardView);
            return DOTween.Sequence();
        }

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView)
        {
            currentSequence?.Kill();
            currentSequence = isStandUp ? DOTween.Sequence() : transform.DOFullMove(_hoverPosition).AppendCallback(() => isStandUp = true);

            int selectedIndex = _allCards.IndexOf(cardView);
            for (int j = 0; j <= LastIndex; j++)
            {
                currentSequence.Join(_allCards[j].transform.DOLocalMoveX((j <= selectedIndex && LastIndex != selectedIndex) ? OFF_SET_EXPAND_Y * (LastIndex - j) - 2 : OFF_SET_EXPAND_Y * (LastIndex - j), ViewValues.FAST_TIME_ANIMATION));
            }

            currentSequence.Join(cardView.transform.DOScale(1.1f, ViewValues.FAST_TIME_ANIMATION))
                .Join(cardView.transform.DOLocalMoveZ(1, ViewValues.FAST_TIME_ANIMATION)).SetEase(Ease.OutCubic);
            return currentSequence;
        }

        public override Tween MouseExit(CardView cardView)
        {
            currentSequence?.Kill();
            currentSequence = transform.DOFullMove(transform.parent).PrependCallback(() => isStandUp = false);

            for (int j = 0; j <= LastIndex; j++)
            {
                currentSequence.Join(_allCards[j].transform.DOLocalMoveX(0, ViewValues.FAST_TIME_ANIMATION));
            }
            cardView.transform.DOScale(1, ViewValues.FAST_TIME_ANIMATION);
            cardView.transform.DOLocalMoveZ(0, ViewValues.FAST_TIME_ANIMATION);
            return currentSequence;
        }
    }
}
