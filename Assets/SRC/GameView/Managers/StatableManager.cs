using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class StatableManager
    {
        [Inject] private readonly List<IStatableView> _allStats;

        /*******************************************************************/
        public void Add(IStatableView statableView) => _allStats.Add(statableView);

        public IStatableView Get(Stat stat) => _allStats.Single(statView => statView.Stat == stat);

        public List<IStatableView> GetAll(Stat stat) => _allStats.FindAll(statView => statView.Stat == stat);
    }
}
