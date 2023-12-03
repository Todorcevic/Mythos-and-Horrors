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
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;

        /*******************************************************************/
        public void MoveCardToZone(Card card, Zone gameZone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView newZoneView = _zonesManager.Get(gameZone);

            DOTween.Sequence()
                .PrependCallback(() => _ioActivatorComponent.DesactivateSensor())
                .OnStart(() => cardView.SetCurrentZoneView(newZoneView))
                .Join(cardView.CurrentZoneView.OutZone(cardView))
                .Join(newZoneView.IntoZone(cardView))
                .AppendCallback(() => _ioActivatorComponent.ActivateSensor());
        }

        public async Task MoveCardToZoneAsync(Card card, Zone gameZone)
        {
            CardView cardView = _cardsManager.Get(card);
            ZoneView newZoneView = _zonesManager.Get(gameZone);

            await DOTween.Sequence()
                 .PrependCallback(() => _ioActivatorComponent.DesactivateSensor())
                .OnStart(() => cardView.SetCurrentZoneView(newZoneView))
                .Join(cardView.CurrentZoneView.OutZone(cardView))
                .Join(newZoneView.IntoZone(cardView))
                .AppendCallback(() => _ioActivatorComponent.ActivateSensor())
                .AsyncWaitForCompletion();
        }
    }
}
