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
        public async Task IncreaseStat(Stat stat)
        {
            IStatableView statable = _statsViewsManager.Get(stat);
            if (statable != null) await statable.IncreaseValue(stat.Value).AsyncWaitForCompletion();
        }

        public async Task DecreaseStat(Stat stat)
        {
            IStatableView statable = _statsViewsManager.Get(stat);
            if (statable != null) await statable.DecreaseValue(stat.Value).AsyncWaitForCompletion();
        }

        public async Task UpdateStat(Stat stat)
        {
            IStatableView statable = _statsViewsManager.Get(stat);
            if (statable != null) await statable.UpdateValue(stat.Value).AsyncWaitForCompletion();
        }
    }
}
