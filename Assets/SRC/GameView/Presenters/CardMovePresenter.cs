using GameRules;
using Zenject;
using DG.Tweening;

namespace GameView
{
    public class CardMovePresenter : ICardMovePresenter
    {
        [Inject] private readonly ZonesManager _zonesManager;
        [Inject] private readonly CardsManager _cardsManager;

        /*******************************************************************/
        public void MoveCardToZone(string cardId, ZoneType gameZone)
        {
            (CardView card, ZoneView zone) = GetCardAndZone(cardId, gameZone);
            zone.MoveCard(card);
        }

        public async void MoveCardToZoneWithPreview(string cardId, ZoneType gameZone)
        {
            (CardView card, ZoneView zone) = GetCardAndZone(cardId, gameZone);
            await _zonesManager.FrontCamera.MoveCard(card).AsyncWaitForCompletion();
            zone.MoveCard(card);
        }

        private (CardView, ZoneView) GetCardAndZone(string cardId, ZoneType gameZone) =>
            (_cardsManager.Get(cardId), _zonesManager.Get(gameZone));
    }
}
