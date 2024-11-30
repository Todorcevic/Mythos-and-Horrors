using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneDiscardView : ZoneView
    {
        public bool IsRightDesplacement;
        private bool isStandUp;
        private Sequence hoverAnimation;
        private readonly List<CardView> _allCards = new();
        [SerializeField, Required] protected Transform _movePosition;
        [SerializeField, Required] protected Transform _hoverPosition;

        private float OFF_SET_EXPAND_X => -1.5f * (IsRightDesplacement ? -1 : 1);
        private float OFF_SET_EXPAND_SELECTED => 2f * (IsRightDesplacement ? -1 : 1);
        private float YOffSet => _allCards.Count * ViewValues.CARD_THICKNESS;
        public int LastIndex => _allCards.Count - 1;

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView)
        {
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            _allCards.Add(cardView);
            return cardView.transform.DOFullLocalMove(_movePosition, ViewValues.DEFAULT_TIME_ANIMATION);
        }

        public override Tween ExitZone(CardView cardView)
        {
            _allCards.Remove(cardView);
            return Repositionate();
        }

        public override Tween MouseEnter(CardView cardView)
        {
            hoverAnimation?.Kill();
            hoverAnimation = isStandUp ? DOTween.Sequence() : transform.DOFullLocalMove(_hoverPosition).AppendCallback(() => isStandUp = true);

            int selectedIndex = _allCards.IndexOf(cardView);
            for (int j = 0; j <= LastIndex; j++)
            {
                hoverAnimation.Join(_allCards[j].transform.DOLocalMoveX((j <= selectedIndex && LastIndex != selectedIndex) ?
                    OFF_SET_EXPAND_X * (LastIndex - j) - OFF_SET_EXPAND_SELECTED : OFF_SET_EXPAND_X * (LastIndex - j),
                    ViewValues.FAST_TIME_ANIMATION));
            }

            hoverAnimation.Join(cardView.transform.DOScale(1.1f, ViewValues.FAST_TIME_ANIMATION))
                .Join(cardView.transform.DOLocalMoveZ(1, ViewValues.FAST_TIME_ANIMATION)).SetEase(Ease.OutCubic);
            return hoverAnimation;
        }

        public override Tween MouseExit(CardView cardView)
        {
            hoverAnimation?.Kill();

            cardView.transform.DOScale(1, ViewValues.FAST_TIME_ANIMATION);
            cardView.transform.DOLocalMoveZ(0, ViewValues.FAST_TIME_ANIMATION);

      
            _movePosition.position = transform.parent.position;
            hoverAnimation = transform.DOFullLocalMove(_movePosition).PrependCallback(() => isStandUp = false);

            for (int j = 0; j <= LastIndex; j++)
            {
                hoverAnimation.Join(_allCards[j].transform.DOLocalMoveX(0, ViewValues.FAST_TIME_ANIMATION));
            }

            return hoverAnimation;
        }

        private Sequence Repositionate()
        {
            Sequence reorderSequence = DOTween.Sequence();
            for (int i = 0; i < _allCards.Count; i++)
            {
                reorderSequence.Join(_allCards[i].transform.DOLocalMoveY(ViewValues.CARD_THICKNESS * i, ViewValues.FAST_TIME_ANIMATION));
            }
            return reorderSequence;
        }
    }
}
