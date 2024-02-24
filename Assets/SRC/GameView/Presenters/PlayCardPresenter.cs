using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PlayCardPresenter : IPresenter<PlayEffectGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ZoneViewsManager _zonesViewManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<PlayEffectGameAction>.PlayAnimationWith(PlayEffectGameAction playEffectGA)
        {
            CardView cardView = _cardViewsManager.GetCardView(playEffectGA.Effect.Card);
            if (!playEffectGA.IsEffectPlayed) await MoveCenterCardEffect(cardView);
            else await ReturnFromCenterShow(cardView);
        }

        private async Task MoveCenterCardEffect(CardView cardView)
        {
            await _moveCardHandler.MoveCardToCenter(cardView);
        }

        private async Task ReturnFromCenterShow(CardView cardView)
        {
            if (cardView.CurrentZoneView == _zonesViewManager.CenterShowZone)
                await _moveCardHandler.ReturnCard(cardView.Card);
        }
    }
}
