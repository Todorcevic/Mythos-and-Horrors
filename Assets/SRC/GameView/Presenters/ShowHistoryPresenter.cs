using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ShowHistoryPresenter : IHistoryShower
    {
        [Inject] private readonly ShowHistoryComponent _historyComponent;
        [Inject] private readonly ActivatorUIPresenter _activatorUIPresenter;


        /*******************************************************************/
        public async Task ShowHistory(History history)
        {
            _activatorUIPresenter.Activate();
            await _historyComponent.Show(history);
            _activatorUIPresenter.Deactivate();
        }
    }
}
