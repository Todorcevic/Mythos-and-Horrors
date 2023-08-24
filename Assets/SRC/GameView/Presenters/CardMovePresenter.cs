using GameRules;
using Zenject;
using DG.Tweening;
using System.Threading.Tasks;

namespace GameView
{
    public class CardMovePresenter : ICardMover
    {
        [Inject] private readonly ZonesManager _zonesManager;
        [Inject] private readonly CardsManager _cardsManager;

        /*******************************************************************/
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
