using Zenject;
using DG.Tweening;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class CardMoverPresenter : ICardMoveAnimator
    {
        [Inject] private readonly ZoneViewsManager _zonesManager;
        [Inject] private readonly CardViewsManager _cardsManager;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly SwapInvestigatorPresenter _swapInvestigatorPresenter;

        /*******************************************************************/
        public async Task MoveCardWithPreviewToZone(Card card, Zone zone)
        {
            await DirectMove(card, _chaptersProvider.CurrentScene.SelectorZone);
            await _swapInvestigatorPresenter.Select(zone);
            await DirectMove(card, zone);
        }

        public async Task MoveCardsToZone(List<Card> cards, Zone zone)
        {
            List<Task> tasks = new();
            await _swapInvestigatorPresenter.Select(zone);

            foreach (Card card in cards)
            {
                await Task.Delay(16);
                tasks.Add(DirectMove(card, zone));
            }

            await Task.WhenAll(tasks);
        }

        public async Task DirectMove(Card card, Zone zone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView newZoneView = _zonesManager.Get(zone);

            Sequence moveSequence = DOTween.Sequence()
                .Join(cardView.CurrentZoneView.ExitZone(cardView))
                .Join(cardView.Rotate())
                .Join(newZoneView.EnterZone(cardView));

            cardView.SetCurrentZoneView(newZoneView);

            await moveSequence.AsyncWaitForCompletion();
        }
    }
}
