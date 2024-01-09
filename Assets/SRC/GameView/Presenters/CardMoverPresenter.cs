using Zenject;
using DG.Tweening;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public class CardMoverPresenter : IAnimatorEnd
    {
        [Inject] private readonly ZoneViewsManager _zonesManager;
        [Inject] private readonly CardViewsManager _cardsManager;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly SwapInvestigatorPresenter _swapInvestigatorPresenter;

        /*******************************************************************/
        public async Task CheckingAtEnd(GameAction gameAction)
        {
            if (gameAction is MoveCardsGameAction moveCardAction)
            {
                if (moveCardAction.IsSingleMove) await MoveCardToZone(moveCardAction.Card, moveCardAction.Zone);
                else await MoveCardsToZone(moveCardAction.Cards, moveCardAction.Zone);
            }
        }

        /*******************************************************************/
        public async Task MoveCardToZone(Card card, Zone zone)
        {
            await RealMove(card, _chaptersProvider.CurrentScene.SelectorZone);
            await _swapInvestigatorPresenter.Select(zone);
            await RealMove(card, zone);
        }

        private async Task MoveCardsToZone(List<Card> cards, Zone zone)
        {
            await _swapInvestigatorPresenter.Select(zone);

            foreach (Card card in cards)
            {
                await Task.Delay(100);
                _ = RealMove(card, zone);
            }
        }

        private async Task RealMove(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView newZoneView = _zonesManager.Get(zone);

            Sequence moveSequence = DOTween.Sequence()
                .Join(cardView.CurrentZoneView.ExitZone(cardView))
                .Join(newZoneView.EnterZone(cardView));

            cardView.SetCurrentZoneView(newZoneView);

            await moveSequence.AsyncWaitForCompletion();
        }
    }
}
