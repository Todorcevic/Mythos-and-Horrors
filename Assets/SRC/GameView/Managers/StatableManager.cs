using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameView
{
    public class StatableManager
    {
        private readonly List<IStatable> _allStats = new();

        /*******************************************************************/
        public void Add(IStatable statView) => _allStats.Add(statView);

        public IStatable Get(Stat stat) => _allStats.Find(statView => statView.Stat == stat);
    }
}
