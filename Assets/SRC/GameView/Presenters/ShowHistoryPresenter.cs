using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryPresenter : IShowHistoryAnimator
    {
        [Inject] private readonly ShowHistoryComponent _historyComponent;
        [Inject] private readonly IOActivatorComponent _ioActivatorComponent;

        /*******************************************************************/
        public async Task ShowHistory(ShowHistoryGameAction showHistoryGameAction)
        {
            _ioActivatorComponent.ActivateUI();
            await _historyComponent.Show(showHistoryGameAction.History);
            showHistoryGameAction.Continue();
            _ioActivatorComponent.DeactivateUI();
        }
    }
}
