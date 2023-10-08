using Zenject;
using DG.Tweening;
using System.Threading.Tasks;
using Sirenix.Utilities;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class CardMovePresenter : ICardMover
    {
        [Inject] private readonly ZonesManager _zonesManager;
        [Inject] private readonly CardsViewManager _cardsManager;

        private ZoneView FrontCameraZone => _zonesManager.Get("FrontCameraZone");

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
            return FrontCameraZone.MoveCard(cardView);
        }

        public void FastMoveCardToZone(Card card, Zone gameZone)
        {
            (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
            zone.MoveCard(cardView);
        }

        public async Task MoveCardToZone(Card card, Zone gameZone)
        {
            (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
            await zone.MoveCard(cardView).AsyncWaitForCompletion();
        }

        public async Task MoveCardToZoneWithPreview(Card card, Zone gameZone)
        {
            (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
            await FrontCameraZone.MoveCard(cardView).AsyncWaitForCompletion();
            await zone.MoveCard(cardView).AsyncWaitForCompletion();
        }

        public async Task FastMoveCardToZoneWithPreview(Card card, Zone gameZone)
        {
            (CardView cardView, ZoneView zone) = GetCardAndZone(card, gameZone);
            await FrontCameraZone.MoveCard(cardView).AsyncWaitForCompletion();
            zone.MoveCard(cardView);
        }

        private (CardView, ZoneView) GetCardAndZone(Card card, Zone gameZone) =>
            (_cardsManager.Get(card), _zonesManager.Get(gameZone));
    }
}
