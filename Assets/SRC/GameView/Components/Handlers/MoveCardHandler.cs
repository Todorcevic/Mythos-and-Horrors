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
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;

        /*******************************************************************/
        public async Task MoveCardWithPreviewToZone(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            await MoveCardWithPreviewToZone(cardView, zoneView);
        }

        public async Task MoveCardWithPreviewToZone(CardView cardView, ZoneView zoneView)
        {
            await cardView.MoveToZone(_zonesViewManager.CenterShowZone, Ease.OutSine).AsyncWaitForCompletion();
            await _swapInvestigatorPresenter.Select(zoneView.Zone);
            await cardView.MoveToZone(zoneView, Ease.InCubic).AsyncWaitForCompletion();
        }

        public async Task MoveCardWithPreviewWithoutWait(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView zoneView = _zonesViewManager.Get(zone);

            await cardView.MoveToZone(_zonesViewManager.CenterShowZone, Ease.OutSine).AsyncWaitForCompletion();
            await _swapInvestigatorPresenter.Select(zone);
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
            await _swapInvestigatorPresenter.Select(zoneView.Zone);
            float delay = 0;
            Sequence sequence = DOTween.Sequence();

            cardViews.ForEach(cardView => sequence.Insert(delay += 0.016f, cardView.MoveToZone(zoneView)));
            await sequence.AsyncWaitForCompletion();
        }
    }
}
