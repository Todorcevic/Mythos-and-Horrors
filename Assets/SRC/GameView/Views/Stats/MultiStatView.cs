using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class MultiStatView : StatView, IMultiStatable
    {
        public List<Stat> MultiStat { get; private set; }
        public override string ValueToShow => (Stat.Value - MultiStat.Sum(stat => stat.Value)).ToString();

        /*******************************************************************/
        public void SetMultiStats(List<Stat> stats)
        {
            MultiStat = stats;
        }
    }
}
