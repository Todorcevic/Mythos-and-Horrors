using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class StatableManager
    {
        [Inject] private List<IStatableView> _allStats;

        /*******************************************************************/
        public void Add(IStatableView statableView)
        {
            if (_allStats.Contains(statableView)) return;
            _allStats.Add(statableView);
        }

        public IStatableView Get(Stat stat) => _allStats.First(statView => statView.Stat == stat);

        public IEnumerable<IStatableView> GetAll(Stat stat) => _allStats.FindAll(statView => statView.Stat == stat);

        public IEnumerable<IStatableView> GetAll(IEnumerable<Stat> stats) => _allStats.FindAll(statView => stats.Contains(statView.Stat));
    }
}
