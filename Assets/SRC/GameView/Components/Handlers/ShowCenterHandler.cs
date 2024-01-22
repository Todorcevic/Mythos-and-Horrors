using DG.Tweening;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowCenterHandler
    {
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;

        private List<CardView> _cardViews = new();
        /*******************************************************************/
        public Tween ShowCenter(List<CardView> cardViews)
        {
            _cardViews = cardViews;
            Sequence showCenterSequence = DOTween.Sequence();
            _cardViews.ForEach(cardView => showCenterSequence.Join(cardView.MoveToZone(_zoneViewsManager.SelectorZone)));
            return showCenterSequence;
        }

        public Tween Return()
        {
            Sequence returnSequence = DOTween.Sequence();
            _cardViews.ForEach(cardView => returnSequence.Join(cardView.MoveToZone(_zoneViewsManager.Get(cardView.Card.CurrentZone))));
            return returnSequence;
        }
    }
}
