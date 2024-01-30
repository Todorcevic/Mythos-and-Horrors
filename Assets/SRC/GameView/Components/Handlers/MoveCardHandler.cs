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
        public Tween MoveCardWithPreviewToZone(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            return MoveCardWithPreviewToZone(cardView, zoneView);
        }

        public Tween MoveCardWithPreviewToZone(CardView cardView, ZoneView zoneView)
        {
            return DOTween.Sequence()
                .Append(_swapInvestigatorHandler.Select(cardView.CurrentZoneView.Zone))
                .Append(cardView.MoveToZone(_zonesViewManager.CenterShowZone, Ease.OutSine))
                .Append(_swapInvestigatorHandler.Select(zoneView.Zone))
                .Append(cardView.MoveToZone(zoneView, Ease.InCubic));
            //await _swapInvestigatorHandler.Select(cardView.CurrentZoneView.Zone).AsyncWaitForCompletion();
            //await cardView.MoveToZone(_zonesViewManager.CenterShowZone, Ease.OutSine).AsyncWaitForCompletion();
            //await _swapInvestigatorHandler.Select(zoneView.Zone).AsyncWaitForCompletion();
            //await cardView.MoveToZone(zoneView, Ease.InCubic).AsyncWaitForCompletion();
        }

        public async Task MoveCardWithPreviewWithoutWait(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            await cardView.MoveToZone(_zonesViewManager.CenterShowZone, Ease.OutSine).AsyncWaitForCompletion();
            await _swapInvestigatorHandler.Select(zone).AsyncWaitForCompletion();
            cardView.MoveToZone(zoneView, Ease.InCubic);
        }

        public async Task MoveCardsToZone(List<Card> cards, Zone zone)
        {
            List<CardView> cardViews = _cardsManager.Get(cards);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            await MoveCardsToZone(cardViews, zoneView);
        }

        public async Task MoveCardsToZone(List<CardView> cardViews, ZoneView zoneView)
        {
            await _swapInvestigatorHandler.Select(zoneView.Zone).AsyncWaitForCompletion();
            float delay = 0;
            Sequence sequence = DOTween.Sequence();

            cardViews.ForEach(cardView => sequence.Insert(delay += 0.016f, cardView.MoveToZone(zoneView)));
            await sequence.AsyncWaitForCompletion();
        }
    }
}
