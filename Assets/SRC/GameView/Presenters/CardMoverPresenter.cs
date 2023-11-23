using Zenject;
using DG.Tweening;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Sirenix.Utilities;

namespace MythsAndHorrors.GameView
{
    public class CardMoverPresenter : ICardMover
    {
        [Inject] private readonly ZonesManager _zonesManager;
        [Inject] private readonly CardsViewsManager _cardsManager;

        private ZoneView SelectorZone => _zonesManager.Get("SelectorZone");

        /*******************************************************************/
        public async Task InstantMoveCardToZone(Zone gameZone, Card[] cards)
        {
            ZoneView zoneView = _zonesManager.Get(gameZone);
            Sequence sequence = DOTween.Sequence();
            cards.ForEach(card => sequence.Append(zoneView.EnterCard(_cardsManager.Get(card), ViewValues.INSTANT_TIME_ANIMATION)));
            await sequence.Play().AsyncWaitForCompletion();
        }

        public async Task MoveCardToZone(Zone gameZone, Card[] cards)
        {
            ZoneView zoneView = _zonesManager.Get(gameZone);
            Sequence sequence = DOTween.Sequence();
            cards.ForEach(card => sequence.Append(zoneView.EnterCard(_cardsManager.Get(card))));
            await sequence.Play().AsyncWaitForCompletion();
        }

        //public async Task MoveCardToZoneWithPreview(Zone gameZone, Card[] cards)
        //{
        //    (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
        //    await SelectorZone.EnterCard(cardView).AsyncWaitForCompletion();
        //    await zone.EnterCard(cardView).AsyncWaitForCompletion();
        //}

        //public async Task FastMoveCardToZoneWithPreview(Zone gameZone, Card[] cards)
        //{
        //    (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
        //    await SelectorZone.EnterCard(cardView).AsyncWaitForCompletion();
        //    zone.EnterCard(cardView);
        //}
    }
}
