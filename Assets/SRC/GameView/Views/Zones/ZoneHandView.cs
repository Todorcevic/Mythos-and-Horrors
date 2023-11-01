using DG.Tweening;
using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class ZoneHandView : ZoneView, IZoneBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private InvisibleHolderView _invisibleHolderView;

        /*******************************************************************/
        public override Tween MoveCard(CardView card)
        {
            return DOTween.Sequence()
            .Join(_invisibleHolderView.AddCardView(card))
            .Join(card.transform.DORotate(transform.eulerAngles, ViewValues.FAST_TIME_ANIMATION))
            .Join(card.transform.DOScale(transform.localScale, ViewValues.FAST_TIME_ANIMATION))
            .OnComplete(() => card.SetCurrentZoneView(this));
        }

        public override Tween RemoveCard(CardView card) => _invisibleHolderView.RemoveCardView(card);

        void IZoneBehaviour.OnMouseDrag(CardView cardView)
        {
            throw new NotImplementedException();
        }

        void IZoneBehaviour.OnMouseEnter(CardView cardView)
        {
            _invisibleHolderView.RepositionateWith(cardView);
            cardView.transform.DOLocalMoveZ(cardView.transform.localPosition.z + 6, ViewValues.FAST_TIME_ANIMATION);
        }

        void IZoneBehaviour.OnMouseExit(CardView cardView)
        {
            _invisibleHolderView.Repositionate();
        }
    }
}
