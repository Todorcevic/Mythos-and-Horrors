using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class ContinuousPresenter : IPresenter<GameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<GameAction>.PlayAnimationWith(GameAction gameAction)
        {
            UpdateCardsState();
            await ReturnFromCenterShow();
        }

        private void UpdateCardsState() => _cardViewsManager.GetAllUpdatable().ForEach(deckCardView => deckCardView.UpdateState());

        private async Task ReturnFromCenterShow() => await _moveCardHandler.ReturnCenterShowCard();
    }
}
