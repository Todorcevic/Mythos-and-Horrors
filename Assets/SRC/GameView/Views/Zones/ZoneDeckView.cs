using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythsAndHorrors.GameView
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
            return DOTween.Sequence();
        }

        public override Tween MouseEnter(CardView cardView)
        {
            _hoverAnimation?.Kill();
            _hoverPosition.localPosition = new Vector3(0, _hoverPosition.localPosition.y + YOffSet, 0);
            _hoverAnimation = _allCards.Last().transform.DOFullLocalMove(_hoverPosition).SetEase(Ease.OutCubic);
            return _hoverAnimation;
        }

        public override Tween MouseExit(CardView cardView)
        {
            _hoverAnimation?.Kill();
            _hoverPosition.localPosition = new Vector3(0, _hoverPosition.localPosition.y - YOffSet, 0);
            _movePosition.localPosition = new Vector3(0, YOffSet, 0);
            _hoverAnimation = _allCards.Last().transform.DOFullLocalMove(_movePosition);
            return _hoverAnimation;
        }

        public override Tween Shuffle()
        {
            _allCards = _allCards.OrderBy(card => Zone.Cards.IndexOf(card.Card)).ToList();

            Sequence ShuffleSequence = DOTween.Sequence();
            for (int i = 0; i < _allCards.Count; i++)
            {
                _allCards[i].transform.SetSiblingIndex(i);
                ShuffleSequence.Insert(ViewValues.FAST_TIME_ANIMATION * 0.1f * i,
                DOTween.Sequence().Join(_allCards[i].transform.DOShakeRotation(ViewValues.DEFAULT_TIME_ANIMATION + 0.001f)) // + 0.001f to avoid warning
                .Join(DOTween.Sequence().Join(_allCards[i].transform.DOLocalMoveX((Random.value - 0.25f), ViewValues.DEFAULT_TIME_ANIMATION * 0.5f))
                .Append(_allCards[i].transform.DOLocalMoveX(0, ViewValues.DEFAULT_TIME_ANIMATION * 0.5f)))
                .Join(_allCards[i].transform.DOLocalMoveY(ViewValues.CARD_THICKNESS * i, ViewValues.DEFAULT_TIME_ANIMATION))
                );
            }

            return ShuffleSequence;
        }
    }
}
