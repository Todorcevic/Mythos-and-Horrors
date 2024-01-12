using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneDiscardView : ZoneView
    {
        private const float OFF_SET_EXPAND_X = -1.5f;
        private const float OFF_SET_EXPAND_SELECTED = 6f;
        [SerializeField, Required] protected Transform _movePosition;
        [SerializeField, Required] protected Transform _hoverPosition;
        private bool isStandUp;
        private Sequence currentSequence;
        private readonly List<CardView> _allCards = new();

        private float YOffSet => _allCards.Count * ViewValues.CARD_THICKNESS;
        private int LastIndex => _allCards.Count - 1;

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView)
        {
            _allCards.Add(cardView);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            return cardView.transform.DOFullMoveDefault(_movePosition);
        }

        public override Tween ExitZone(CardView cardView)
        {
            _allCards.Remove(cardView);
            return DOTween.Sequence();
        }

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView)
        {
            currentSequence?.Kill();
            currentSequence = isStandUp ? DOTween.Sequence() : transform.DOFullLocalMove(_hoverPosition).AppendCallback(() => isStandUp = true);

            int selectedIndex = _allCards.IndexOf(cardView);
            for (int j = 0; j <= LastIndex; j++)
            {
                currentSequence.Join(_allCards[j].transform.DOLocalMoveX((j <= selectedIndex && LastIndex != selectedIndex) ? OFF_SET_EXPAND_X * (LastIndex - j) - OFF_SET_EXPAND_SELECTED : OFF_SET_EXPAND_X * (LastIndex - j), ViewValues.FAST_TIME_ANIMATION));
            }

            currentSequence.Join(cardView.transform.DOScale(1.1f, ViewValues.FAST_TIME_ANIMATION))
                .Join(cardView.transform.DOLocalMoveZ(1, ViewValues.FAST_TIME_ANIMATION)).SetEase(Ease.OutCubic);
            return currentSequence;
        }

        public override Tween MouseExit(CardView cardView)
        {
            cardView.transform.DOScale(1, ViewValues.FAST_TIME_ANIMATION);
            cardView.transform.DOLocalMoveZ(0, ViewValues.FAST_TIME_ANIMATION);

            currentSequence?.Kill();
            _movePosition.position = transform.parent.position;
            currentSequence = transform.DOFullLocalMove(_movePosition).PrependCallback(() => isStandUp = false);

            for (int j = 0; j <= LastIndex; j++)
            {
                currentSequence.Join(_allCards[j].transform.DOLocalMoveX(0, ViewValues.FAST_TIME_ANIMATION));
            }

            return currentSequence;
        }
    }
}
