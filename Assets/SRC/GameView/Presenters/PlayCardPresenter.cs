using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PlayCardPresenter : IPresenter<PlayEffectGameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<PlayEffectGameAction>.PlayAnimationWith(PlayEffectGameAction playEffectGA)
        {
            await MoveCenterCardEffect(playEffectGA);
        }

        private async Task MoveCenterCardEffect(PlayEffectGameAction playEffectGA)
        {
            CardView cardView = _cardViewsManager.GetCardView(playEffectGA.Effect.Card);
            if (playEffectGA.Effect.WithReturn) await _moveCardHandler.ReturnCard(cardView.Card);
            else await _moveCardHandler.MoveCardToCenter(cardView);
        }
    }
}
