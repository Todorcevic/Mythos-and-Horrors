using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class StatGameAction : GameAction
    {
        private readonly Dictionary<Stat, int> _statsWithOldValue;
        [Inject] private readonly IPresenter<StatGameAction> _StatsPresenter;

        public Dictionary<Stat, int> StatsWithValue { get; }
        public IEnumerable<Stat> AllStats => StatsWithValue.Keys.ToList();

        /*******************************************************************/
        public StatGameAction(Stat stat, int value) : this(new Dictionary<Stat, int> { { stat, value } }) { }

        public StatGameAction(Dictionary<Stat, int> statsWithValues)
        {
            _statsWithOldValue = statsWithValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Key.Value);
            StatsWithValue = statsWithValues;
            CanBeExecuted = StatsWithValue.Count > 0;
        }

        /*******************************************************************/
        public bool HasStat(Stat stat) => StatsWithValue.ContainsKey(stat);

        protected override async Task ExecuteThisLogic()
        {
            await _StatsPresenter.PlayAnimationWith(this);
        }

        protected override async Task UndoThisLogic()
        {
            StatsWithValue.ForEach(stat => stat.Key.UpdateValue(_statsWithOldValue[stat.Key]));
            await _StatsPresenter.PlayAnimationWith(this);
        }
    }
}
