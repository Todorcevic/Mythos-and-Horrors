using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryPresenter : IAnimator
    {
        [Inject] private readonly ShowHistoryComponent _historyComponent;
        [Inject] private readonly ActivatorUIPresenter _activatorUIPresenter;

        /*******************************************************************/
        public async Task Checking(GameAction gameAction)
        {
            if (gameAction is ShowHistoryGameAction showHistoryGameAction)
                await ShowHistory(showHistoryGameAction);
        }

        /*******************************************************************/
        public async Task ShowHistory(ShowHistoryGameAction showHistoryGameAction)
        {
            _activatorUIPresenter.ActivateUI();
            await _historyComponent.Show(showHistoryGameAction.History);
            _activatorUIPresenter.Deactivate();
            showHistoryGameAction.Continue();
        }
    }
}
