using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryPresenter : IShowHistoryAnimator
    {
        [Inject] private readonly ShowHistoryComponent _historyComponent;
        [Inject] private readonly ActivatorUIPresenter _activatorUIPresenter;

        /*******************************************************************/
        public async Task ShowHistory(ShowHistoryGameAction showHistoryGameAction)
        {
            _activatorUIPresenter.ActivateUI();
            await _historyComponent.Show(showHistoryGameAction.History);
            showHistoryGameAction.Continue();
            _activatorUIPresenter.DeactivateUI();
        }
    }
}
