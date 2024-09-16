using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class UpdateStatGameAction : GameAction
    {
        private Dictionary<Stat, int> _statsWithOldValue;

        public Dictionary<Stat, int> StatsWithValue { get; private set; }
        public List<Stat> AllStatsUpdated => StatsWithValue.Keys.ToList();

        public override bool CanBeExecuted => StatsWithValue.Count > 0;

        /*******************************************************************/
        public UpdateStatGameAction SetWith(Stat stat, int value) => SetWith(new Dictionary<Stat, int> { { stat, value } });

        public UpdateStatGameAction SetWith(Dictionary<Stat, int> statsWithValues)
        {
            StatsWithValue = statsWithValues.Where(kv => kv.Key.Value != kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);
            return this;
        }

        /*******************************************************************/
        public bool HasThisStat(Stat stat) => AllStatsUpdated.Contains(stat);

        protected override async Task ExecuteThisLogic()
        {
            _statsWithOldValue = StatsWithValue.ToDictionary(statNewValues => statNewValues.Key, kvp => kvp.Key.Value);
            StatsWithValue.ForEach(statNewValues => statNewValues.Key.UpdateValue(statNewValues.Value));
            await Task.CompletedTask;
        }

        public override async Task Undo()
        {
            StatsWithValue.ForEach(statNewValues => statNewValues.Key.UpdateValue(_statsWithOldValue[statNewValues.Key]));
            await base.Undo();
        }
    }
}
