using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ZoneDeckView : ZoneView
    {
        private Tween _hoverAnimation;
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _movePosition;
        [SerializeField, Required, ChildGameObjectsOnly] protected Transform _hoverPosition;
        private List<CardView> _allCards = new();
        private float YOffSet => _allCards.Count * ViewValues.CARD_THICKNESS;

        /*******************************************************************/
        public override Tween EnterZone(CardView cardView)
        {
            _allCards.Add(cardView);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            return cardView.transform.DOFullLocalMove(_movePosition, ViewValues.DEFAULT_TIME_ANIMATION);
        }

        public override Tween ExitZone(CardView cardView)
        {
            _allCards.Remove(cardView);
            return Repositionate();
        }

        public override Tween MouseEnter(CardView cardView)
        {
            _hoverAnimation?.Kill();
            _hoverPosition.localPosition = new Vector3(0, _hoverPosition.localPosition.y + YOffSet, 0);
            return _hoverAnimation = _allCards.Last().transform.DOFullLocalMove(_hoverPosition).SetEase(Ease.OutCubic);
        }

        public override Tween MouseExit(CardView cardView)
        {
            _hoverAnimation?.Kill();
            _hoverPosition.localPosition = new Vector3(0, _hoverPosition.localPosition.y - YOffSet, 0);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            return _hoverAnimation = _allCards.Last().transform.DOFullLocalMove(_movePosition);
        }

        public override Tween Shuffle()
        {
            _allCards = _allCards.OrderBy(card => Zone.Cards.IndexOf(card.Card)).ToList();

            Sequence ShuffleSequence = DOTween.Sequence();
            for (int i = 0; i < _allCards.Count; i++)
            {
                _allCards[i].transform.SetSiblingIndex(i);
                ShuffleSequence.Insert(ViewValues.FAST_TIME_ANIMATION * 0.1f * i,
                DOTween.Sequence().Join(_allCards[i].Rotate())
                .Join(_allCards[i].transform.DOShakeRotation(ViewValues.DEFAULT_TIME_ANIMATION + 0.001f))
                .Join(DOTween.Sequence().Join(_allCards[i].transform.DOLocalMoveX(Random.value - 0.25f, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f))
                .Append(_allCards[i].transform.DOLocalMoveX(0, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f))));
            }
            ShuffleSequence.Join(Repositionate());
            return ShuffleSequence;
        }

        public override Tween UpdatePosition(Card cardToChange)
        {
            int newPosition = cardToChange.CurrentZone.Cards.IndexOf(cardToChange);
            CardView cardViewToChange = _allCards.Find(card => card.Card == cardToChange);
            _allCards.Remove(cardViewToChange);
            _allCards.Insert(newPosition, cardViewToChange);
            cardViewToChange.transform.SetSiblingIndex(newPosition);

            Sequence changePositionSequence = DOTween.Sequence();
            changePositionSequence.Append(cardViewToChange.transform.DOLocalMoveX(-10, ViewValues.DEFAULT_TIME_ANIMATION));
            changePositionSequence.Append(cardViewToChange.transform.DOLocalMoveY(ViewValues.CARD_THICKNESS * newPosition, ViewValues.DEFAULT_TIME_ANIMATION));
            changePositionSequence.Join(Repositionate());
            changePositionSequence.Append(cardViewToChange.transform.DOLocalMoveX(0, ViewValues.DEFAULT_TIME_ANIMATION));
            return changePositionSequence;
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
