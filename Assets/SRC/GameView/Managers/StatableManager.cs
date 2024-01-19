using MythsAndHorrors.GameRules;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public class StatableManager
    {
        private readonly List<IStatableView> _allStats = new();

        /*******************************************************************/
        public void Add(IStatableView statView) => _allStats.Add(statView);

        public IStatableView Get(Stat stat) => _allStats.Find(statView => statView.Stat == stat);
    }
}
