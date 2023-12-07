using Zenject;
using DG.Tweening;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

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

            var asd = DOTween.Sequence()
                .Join(cardView.CurrentZoneView.ExitZone(cardView))
                .Join(newZoneView.EnterZone(cardView));

            cardView.SetCurrentZoneView(newZoneView);

            await asd.AsyncWaitForCompletion();

            //void Start()
            //{
            //    cardView.SetCurrentZoneView(newZoneView);
            //}
        }

        public async Task MoveCardsToZoneAsync(Card[] cards, Zone zone)
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
