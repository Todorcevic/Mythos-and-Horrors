using Zenject;
using DG.Tweening;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public class CardMoverPresenter : ICardMover
    {
        [Inject] private readonly ZoneViewsManager _zonesManager;
        [Inject] private readonly CardViewsManager _cardsManager;

        /*******************************************************************/
        public async Task MoveCardToZoneAsync(Card card, Zone gameZone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView newZoneView = _zonesManager.Get(gameZone);

            Sequence moveSequence = DOTween.Sequence()
                .Join(cardView.CurrentZoneView.ExitZone(cardView))
                .Join(newZoneView.EnterZone(cardView));

            cardView.SetCurrentZoneView(newZoneView);

            await moveSequence.AsyncWaitForCompletion();
        }

        public async Task MoveCardsToZoneAsync(List<Card> cards, Zone zone)
        {
            int delay = 100;

            foreach (Card card in cards)
            {
                await Task.Delay(delay);
                _ = MoveCardToZoneAsync(card, zone);
            }
        }
    }
}
