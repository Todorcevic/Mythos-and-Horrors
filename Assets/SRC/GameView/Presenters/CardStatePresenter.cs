using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class CardStatePresenter : IPresenter
    {
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gameAction)
        {
            _cardViewsManager.GetAllUpdatable().ForEach(deckCardView => deckCardView.UpdateState());
            await Task.CompletedTask;
        }
    }
}
