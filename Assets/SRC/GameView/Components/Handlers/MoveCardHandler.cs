using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class MoveCardHandler
    {
        [Inject] private readonly CardViewsManager _cardsManager;
        [Inject] private readonly ZoneViewsManager _zonesViewManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;

        /*******************************************************************/
        public Tween MoveCardWithPreviewToZone(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.GetCardView(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            return MoveCardViewWithPreviewToZone(cardView, zoneView);
        }

        public Tween MoveCardViewWithPreviewToZone(CardView cardView, ZoneView zoneView)
        {
            return DOTween.Sequence()
             .Append(MoveCardViewToCenter(cardView))
             .Append(_swapInvestigatorHandler.Select(zoneView.Zone))
             .Append(cardView.MoveToZone(zoneView, Ease.InCubic));
        }

        public Tween MoveCardWithPreviewWithoutWait(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.GetCardView(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);
            return DOTween.Sequence()
                .Append(MoveCardViewToCenter(cardView))
                .Append(_swapInvestigatorHandler.Select(zoneView.Zone))
                .OnComplete(() => cardView.MoveToZone(zoneView, Ease.InCubic));
        }

        public Tween MoveCardtoCenter(Card card)
        {
            CardView cardView = _cardsManager.GetCardView(card);
            return MoveCardViewToCenter(cardView);
        }

        public Tween MoveCardViewToCenter(CardView cardView)
        {
            if (cardView.CurrentZoneView == _zonesViewManager.CenterShowZone) return DOTween.Sequence();

            return DOTween.Sequence()
                .Append(_swapInvestigatorHandler.Select(cardView.CurrentZoneView.Zone))
                .Append(cardView.MoveToZone(_zonesViewManager.CenterShowZone, Ease.OutSine));
        }

        public Tween MoveCardsToZone(IEnumerable<Card> cards, Zone zone, float delay = 0f)
        {
            IEnumerable<CardView> cardViews = _cardsManager.GetCardViews(cards);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            return MoveCardViewsToZone(cardViews, zoneView, delay);
        }

        private Tween MoveCardViewsToZone(IEnumerable<CardView> cardViews, ZoneView zoneView, float delay)
        {
            float delayBetweenMoves = 0f;
            Sequence sequence = DOTween.Sequence();
            cardViews.ForEach(cardView => sequence.Insert(delayBetweenMoves += delay, cardView.MoveToZone(zoneView)));
            return DOTween.Sequence().Append(_swapInvestigatorHandler.Select(zoneView.Zone)).Append(sequence);
        }

        public Tween ReturnCard(Card card) => MoveCardWithPreviewToZone(card, card.CurrentZone);

        public Tween MoveCardsToZones(Dictionary<Card, Zone> cards, float delay = 0f)
        {
            IEnumerable<CardView> cardViews = _cardsManager.GetCardViews(cards.Keys);
            IEnumerable<ZoneView> zoneViews = _zonesViewManager.Get(cards.Values);

            return MoveCardViewsToZones(cardViews, zoneViews, delay);
        }

        private Tween MoveCardViewsToZones(IEnumerable<CardView> cardViews, IEnumerable<ZoneView> zoneViews, float delay)
        {
            float delayBetweenMoves = 0f;
            Sequence sequence = DOTween.Sequence();
            cardViews.ForEach((cardView, position) => sequence.Insert(delayBetweenMoves += delay, cardView.MoveToZone(zoneViews.ElementAt(position))));
            return DOTween.Sequence().Append(_swapInvestigatorHandler.Select(cardViews.First().Card.CurrentZone)).Append(sequence);
        }
    }
}
