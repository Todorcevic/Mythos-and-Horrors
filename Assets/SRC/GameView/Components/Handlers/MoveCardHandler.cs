using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class MoveCardHandler
    {
        [Inject] private readonly CardViewsManager _cardsManager;
        [Inject] private readonly ZoneViewsManager _zonesViewManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;

        /*******************************************************************/
        public async Task MoveCardWithPreviewToZone(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            await MoveCardWithPreviewToZone(cardView, zoneView);
        }

        public async Task MoveCardWithPreviewToZone(CardView cardView, ZoneView zoneView)
        {
            if (cardView.CurrentZoneView != _zonesViewManager.CenterShowZone) await MoveCardToCenter(cardView);
            await _swapInvestigatorHandler.Select(zoneView.Zone).AsyncWaitForCompletion();
            await cardView.MoveToZone(zoneView, Ease.InCubic).AsyncWaitForCompletion();
        }

        public async Task MoveCardWithPreviewWithoutWait(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            if (cardView.CurrentZoneView != _zonesViewManager.CenterShowZone) await MoveCardToCenter(cardView);
            await _swapInvestigatorHandler.Select(zone).AsyncWaitForCompletion();
            cardView.MoveToZone(zoneView, Ease.InCubic);
        }

        public async Task MoveCardToCenter(CardView cardView)
        {
            await _swapInvestigatorHandler.Select(cardView.CurrentZoneView.Zone).AsyncWaitForCompletion();
            await cardView.MoveToZone(_zonesViewManager.CenterShowZone, Ease.OutSine).AsyncWaitForCompletion();
        }

        public async Task MoveCardsToZone(List<Card> cards, Zone zone, float delay = 0f)
        {
            List<CardView> cardViews = _cardsManager.Get(cards);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            await MoveCardsToZone(cardViews, zoneView, delay);
        }

        private async Task MoveCardsToZone(List<CardView> cardViews, ZoneView zoneView, float delay)
        {
            await _swapInvestigatorHandler.Select(zoneView.Zone).AsyncWaitForCompletion();
            float delayBetweenMoves = 0f;
            Sequence sequence = DOTween.Sequence();
            cardViews.ForEach(cardView => sequence.Insert(delayBetweenMoves += delay, cardView.MoveToZone(zoneView)));
            await sequence.AsyncWaitForCompletion();
        }
    }
}
