using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class StatableManager
    {
        private readonly List<IStatableView> _allStats = new();

        /*******************************************************************/
        public void Add(IStatableView statableView)
        {
            if (_allStats.Contains(statableView)) return;
            _allStats.Add(statableView);
        }

        public IStatableView Get(Stat stat) => _allStats.Single(statView => statView.Stat == stat);

        public List<IStatableView> GetAll(Stat stat) => _allStats.FindAll(statView => statView.Stat == stat);
    }
}
