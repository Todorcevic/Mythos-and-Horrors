using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class FullStatePresenter : IPresenter<GameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<GameAction>.PlayAnimationWith(GameAction gameAction)
        {
            UpdateCardsState();
            await ReturnAvatar();
        }

        private void UpdateCardsState() => _cardViewsManager.GetAllUpdatable().ForEach(deckCardView => deckCardView.UpdateState());

        private async Task ReturnAvatar() => await _moveCardHandler.ReturnCenterShowCard();
    }
}
