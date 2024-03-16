using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class StatGameAction : GameAction
    {
        [Inject] private readonly IPresenter<StatGameAction> _StatsPresenter;

        public Dictionary<Stat, int> StatsWithValue { get; }
        public IEnumerable<Stat> AllStats => StatsWithValue.Keys.ToList();

        /*******************************************************************/
        public StatGameAction(Stat stat, int value) : this(new Dictionary<Stat, int> { { stat, value } }) { }

        public StatGameAction(Dictionary<Stat, int> statsWithValues)
        {
            StatsWithValue = statsWithValues;
        }

        /*******************************************************************/
        public bool HasStat(Stat stat) => StatsWithValue.ContainsKey(stat);

        protected override async Task ExecuteThisLogic()
        {
            await _StatsPresenter.PlayAnimationWith(this);
        }
    }
}
