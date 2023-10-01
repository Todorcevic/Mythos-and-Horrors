using GameRules;
using Zenject;
using DG.Tweening;
using System.Threading.Tasks;
using Sirenix.Utilities;

namespace Tuesday
{
    public class CardMovePresenter : ICardMover
    {
        [Inject] private readonly ZonesManager _zonesManager;
        [Inject] private readonly CardsViewManager _cardsManager;

        /*******************************************************************/
        public async Task MoveCardsInFront(params Card[] cards)
        {
            Sequence sequence = DOTween.Sequence();
            cards.ForEach(card => sequence.Append(MoveCardInFront(card)));
            await sequence.Play().AsyncWaitForCompletion();
        }

        private Tween MoveCardInFront(Card card)
        {
            CardView cardView = _cardsManager.Get(card);
            return _zonesManager.FrontCamera.MoveCard(cardView);
        }

        public void FastMoveCardToZone(Card card, ZoneType gameZone)
        {
            (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
            zone.MoveCard(cardView);
        }

        public async Task MoveCardToZone(Card card, ZoneType gameZone)
        {
            (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
            await zone.MoveCard(cardView).AsyncWaitForCompletion();
        }

        public async Task MoveCardToZoneWithPreview(Card card, ZoneType gameZone)
        {
            (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
            await _zonesManager.FrontCamera.MoveCard(cardView).AsyncWaitForCompletion();
            await zone.MoveCard(cardView).AsyncWaitForCompletion();
        }

        public async Task FastMoveCardToZoneWithPreview(Card card, ZoneType gameZone)
        {
            (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
            await _zonesManager.FrontCamera.MoveCard(cardView).AsyncWaitForCompletion();
            zone.MoveCard(cardView);
        }

        private (CardView, ZoneView) GetCardAndZone(Card card, ZoneType gameZone) =>
            (_cardsManager.Get(card), _zonesManager.Get(gameZone));
    }
}
