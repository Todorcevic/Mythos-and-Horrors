using DG.Tweening;
using MythsAndHorrors.GameRules;
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
            IStatableView statable = _statsViewsManager.Get(updateStatGameAction.Stat);

            if (statable != null) await statable.UpdateValue().AsyncWaitForCompletion();
        }
    }
}
