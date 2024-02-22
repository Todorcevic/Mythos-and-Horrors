using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class CardStatePresenter : INewPresenter<GameAction>
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        async Task INewPresenter<GameAction>.PlayAnimationWith(GameAction gameAction)
        {
            _cardViewsManager.GetAllUpdatable().ForEach(deckCardView => deckCardView.UpdateState());
            await Task.CompletedTask;
        }
    }
}
