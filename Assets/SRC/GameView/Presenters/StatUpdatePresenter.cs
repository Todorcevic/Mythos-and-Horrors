using DG.Tweening;
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
                await UpdateStat(updateStatGameAction).AsyncWaitForCompletion();
        }

        /*******************************************************************/
        private Sequence UpdateStat(StatGameAction updateStatGameAction)
        {
            List<IStatableView> statables = _statsViewsManager.GetAll(updateStatGameAction.Stat);
            Sequence updateStatsSequence = DOTween.Sequence();
            statables.ForEach(statView => updateStatsSequence.Join(statView.UpdateValue()));
            return updateStatsSequence;
        }
    }
}
