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
        public void MoveCardToZone(Card card, Zone gameZone)
        {
            ZoneView zoneView = _zonesManager.Get(gameZone);
            zoneView.EnterCard(_cardsManager.Get(card));
        }

        public async Task MoveCardToZoneAsync(Card card, Zone gameZone)
        {
            ZoneView zoneView = _zonesManager.Get(gameZone);
            await zoneView.EnterCard(_cardsManager.Get(card)).AsyncWaitForCompletion();
        }
    }
}
