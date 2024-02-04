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
        public async Task UpdateStat(StatGameAction updateStatGameAction)
        {
            List<IStatableView> statables = _statsViewsManager.GetAll(updateStatGameAction.Stat);
            Sequence sequence = DOTween.Sequence();
            statables.ForEach(statView => sequence.Join(statView.UpdateValue()));
            await sequence.AsyncWaitForCompletion();
        }
    }
}
