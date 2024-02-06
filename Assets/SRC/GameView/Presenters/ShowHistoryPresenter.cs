using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryPresenter : IPresenter
    {
        [Inject] private readonly ShowHistoryComponent _historyComponent;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gamAction)
        {
            if (gamAction is ShowHistoryGameAction showHistoryGameAction)
                await ShowHistory(showHistoryGameAction);
        }

        /*******************************************************************/
        private async Task ShowHistory(ShowHistoryGameAction showHistoryGameAction)
        {
            await _historyComponent.Show(showHistoryGameAction.History);
        }
    }
}
