using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class StatableManager
    {
        [Inject] private readonly List<IStatable> _allStats;

        /*******************************************************************/
        public void Add(IStatable statableView)
        {
            if (_allStats.Contains(statableView)) return;
            _allStats.Add(statableView);
        }

        public IStatable Get(Stat stat) => _allStats.First(statable => statable.Stat == stat ||
            ((statable is IMultiStatable multistatable) && multistatable.MultiStat.Contains(stat)));

        public IEnumerable<IStatable> GetAll(Stat stat) => _allStats.FindAll(statable => statable.Stat == stat ||
            ((statable is IMultiStatable multistatable) && multistatable.MultiStat.Contains(stat)));

        public IEnumerable<IStatable> GetAll(IEnumerable<Stat> stats) =>
            _allStats.FindAll(statable => stats.Contains(statable.Stat) ||
            ((statable is IMultiStatable multistatable) && multistatable.MultiStat.Intersect(stats).Any()));
    }
}
