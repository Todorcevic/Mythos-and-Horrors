using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class StatUpdatePresenter : IPresenter
    {
        [Inject] private readonly StatableManager _statsViewsManager;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gamAction)
        {
            if (gamAction is StatGameAction updateStatGameAction)
                UpdateStat(updateStatGameAction);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        private void UpdateStat(StatGameAction updateStatGameAction)
        {
            List<IStatableView> statables = _statsViewsManager.GetAll(updateStatGameAction.Stat);
            statables.ForEach(statView => statView.UpdateValue());
        }
    }
}
