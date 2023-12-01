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
            CardView cardView = _cardsManager.Get(card);
            ZoneView newZoneView = _zonesManager.Get(gameZone);

            DOTween.Sequence().Join(cardView.CurrentZoneView.ExitCard(cardView))
           .Join(newZoneView.EnterCard(cardView)).OnStart(() => cardView.SetCurrentZoneView(newZoneView));
        }

        public async Task MoveCardToZoneAsync(Card card, Zone gameZone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView newZoneView = _zonesManager.Get(gameZone);

            await DOTween.Sequence().Join(cardView.CurrentZoneView.ExitCard(cardView))
            .Join(newZoneView.EnterCard(cardView)).OnStart(() => cardView.SetCurrentZoneView(newZoneView))
            .AsyncWaitForCompletion();
        }
    }
}
