using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class ContinuousPresenter : IPresenter<GameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        async Task IPresenter<GameAction>.PlayAnimationWith(GameAction gameAction)
        {
            UpdateCardsState();
            await Task.CompletedTask;
        }

        private void UpdateCardsState() => _cardViewsManager.GetAllUpdatable().ForEach(deckCardView => deckCardView.UpdateState());
    }
}
