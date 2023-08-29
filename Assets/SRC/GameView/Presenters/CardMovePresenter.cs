using GameRules;
using Zenject;
using DG.Tweening;
using System.Threading.Tasks;
using Sirenix.Utilities;

namespace GameView
{
    public class CardMovePresenter : ICardMover
    {
        [Inject] private readonly ZonesManager _zonesManager;
        [Inject] private readonly CardsManager _cardsManager;

        /*******************************************************************/
        public async Task MoveCardsInFront(params string[] cardIds)
        {
            Sequence sequence = DOTween.Sequence();
            cardIds.ForEach(cardId => sequence.Append(MoveCardInFront(cardId)));
            await sequence.Play().AsyncWaitForCompletion();
        }

        private Tween MoveCardInFront(string cardId)
        {
            CardView card = _cardsManager.Get(cardId);
            return _zonesManager.FrontCamera.MoveCard(card);
        }

        public void FastMoveCardToZone(string cardId, ZoneType gameZone)
        {
            (CardView card, ZoneView zone) = GetCardAndZone(cardId, gameZone);
            zone.MoveCard(card);
        }

        public async Task MoveCardToZone(string cardId, ZoneType gameZone)
        {
            (CardView card, ZoneView zone) = GetCardAndZone(cardId, gameZone);
            await zone.MoveCard(card).AsyncWaitForCompletion();
        }

        public async Task MoveCardToZoneWithPreview(string cardId, ZoneType gameZone)
        {
            (CardView card, ZoneView zone) = GetCardAndZone(cardId, gameZone);
            await _zonesManager.FrontCamera.MoveCard(card).AsyncWaitForCompletion();
            await zone.MoveCard(card).AsyncWaitForCompletion();
        }

        public async Task FastMoveCardToZoneWithPreview(string cardId, ZoneType gameZone)
        {
            (CardView card, ZoneView zone) = GetCardAndZone(cardId, gameZone);
            await _zonesManager.FrontCamera.MoveCard(card).AsyncWaitForCompletion();
            zone.MoveCard(card);
        }

        private (CardView, ZoneView) GetCardAndZone(string cardId, ZoneType gameZone) =>
            (_cardsManager.Get(cardId), _zonesManager.Get(gameZone));
    }
}
