using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PlayCardPresenter : IPresenter<PlayEffectGameAction>
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<PlayEffectGameAction>.PlayAnimationWith(PlayEffectGameAction playEffectGA)
        {
            await MoveCenterCardEffect(playEffectGA);
        }

        private async Task MoveCenterCardEffect(PlayEffectGameAction playEffectGA)
        {
            //Card asda = _cardsProvider.GetCardWithThisEffect(playEffectGA.Effect);
            //CardView adsdsa = _cardViewsManager.GetCardView(asda);
            //await _moveCardHandler.MoveCardToCenter(adsdsa);
            await Task.CompletedTask;
        }

    }
}
