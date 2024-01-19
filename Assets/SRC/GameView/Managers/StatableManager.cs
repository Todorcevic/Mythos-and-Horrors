using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class StatableManager
    {
        [Inject] private readonly List<IStatableView> _allStats;

        /*******************************************************************/
        public void Add(IStatableView statView) => _allStats.Add(statView);

        public IStatableView Get(Stat stat) => _allStats.Find(statView => statView.Stat == stat);
    }
}
