using Zenject;
using DG.Tweening;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public class CardMoverPresenter : IPresenter
    {
        [Inject] private readonly ZoneViewsManager _zonesManager;
        [Inject] private readonly CardViewsManager _cardsManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorPresenter;

        /*******************************************************************/
        public async Task MoveCardWith(MoveCardsGameAction moveCardsGameAction)
        {
            ZoneView zoneView = _zonesManager.Get(moveCardsGameAction.Zone);
            if (!moveCardsGameAction.IsSingleMove)
            {
                List<CardView> cardViews = _cardsManager.Get(moveCardsGameAction.Cards);
                await MoveCardsToZone(cardViews, zoneView);
                return;
            }

            CardView cardView = _cardsManager.Get(moveCardsGameAction.Card);
            if (moveCardsGameAction.Parent.Parent is InitialDrawGameAction)
            {
                await MoveCardWithPreviewWithoutWait(cardView, zoneView);
                return;
            }
            await MoveCardWithPreviewToZone(cardView, zoneView);
        }

        private async Task MoveCardWithPreviewToZone(CardView card, ZoneView zone)
        {
            await card.MoveToZone(_zonesManager.CenterShowZone, Ease.OutSine).AsyncWaitForCompletion();
            await _swapInvestigatorPresenter.Select(zone.Zone);
            await card.MoveToZone(zone, Ease.InCubic).AsyncWaitForCompletion();
        }

        private async Task MoveCardWithPreviewWithoutWait(CardView card, ZoneView zone)
        {
            await card.MoveToZone(_zonesManager.CenterShowZone, Ease.OutSine).AsyncWaitForCompletion();
            await _swapInvestigatorPresenter.Select(zone.Zone);
            card.MoveToZone(zone, Ease.InCubic);
        }

        private async Task MoveCardsToZone(List<CardView> cards, ZoneView zone)
        {
            await _swapInvestigatorPresenter.Select(zone.Zone);
            float delay = 0;
            Sequence sequence = DOTween.Sequence();
            cards.ForEach(card => sequence.Insert(delay += 0.016f, card.MoveToZone(zone)));
            await sequence.AsyncWaitForCompletion();
        }
    }
}
