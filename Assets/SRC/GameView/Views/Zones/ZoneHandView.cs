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
    public class ZoneHandView : ZoneView, IZoneBahaviour
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

        void IZoneBahaviour.OnClicked(CardView cardView)
        {
            throw new NotImplementedException();
        }

        void IZoneBahaviour.OnMouseDrag(CardView cardView)
        {
            throw new NotImplementedException();
        }

        void IZoneBahaviour.OnMouseEnter(CardView cardView)
        {
            Debug.Log("OnMouseEnter - Inteface");
            _invisibleHolderView.ShowCard(cardView);
        }

        void IZoneBahaviour.OnMouseExit(CardView cardView)
        {
            Debug.Log("OnMouseExtit - Inteface");
            _invisibleHolderView.HideCard(cardView);
        }
    }
}
