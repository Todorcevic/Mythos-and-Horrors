using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneDiscardView : ZoneView
    {
        private const float OFF_SET_EXPAND_X = -1.5f;
        public float OFF_SET_EXPAND_SELECTED = 2f;
        private bool isStandUp;
        private Sequence hoverAnimation;
        private readonly List<CardView> _allCards = new();
        [SerializeField, Required] protected Transform _movePosition;
        [SerializeField, Required] protected Transform _hoverPosition;

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

        private Tween Repositionate()
        {
            Sequence seq = DOTween.Sequence();
            _allCards.ForEach((card) =>
            {
                int index = _allCards.IndexOf(card);
                seq.Join(card.transform.DOLocalMoveY(index * ViewValues.CARD_THICKNESS, ViewValues.FAST_TIME_ANIMATION));
            });
            return seq;
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
            cardView.transform.DOScale(1, ViewValues.FAST_TIME_ANIMATION);
            cardView.transform.DOLocalMoveZ(0, ViewValues.FAST_TIME_ANIMATION);

            hoverAnimation?.Kill();
            _movePosition.position = transform.parent.position;
            hoverAnimation = transform.DOFullLocalMove(_movePosition).PrependCallback(() => isStandUp = false);

            for (int j = 0; j <= LastIndex; j++)
            {
                hoverAnimation.Join(_allCards[j].transform.DOLocalMoveX(0, ViewValues.FAST_TIME_ANIMATION));
            }

            return hoverAnimation;
        }
    }
}
