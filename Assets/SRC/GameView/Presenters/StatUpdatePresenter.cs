using DG.Tweening;
using MythsAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class StatUpdatePresenter : IStatAnimator
    {
        [Inject] private readonly StatableManager _statsViewsManager;

        /*******************************************************************/
        public async Task UpdateStat(Stat stat)
        {
            IStatable statable = _statsViewsManager.Get(stat);
            if (statable != null) await statable.UpdateValue(stat.Value).AsyncWaitForCompletion();
        }
    }
}
