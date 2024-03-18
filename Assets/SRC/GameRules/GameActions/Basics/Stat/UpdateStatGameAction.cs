using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class UpdateStatGameAction : GameAction
    {
        protected Dictionary<Stat, int> _statsWithOldValue;
        [Inject] private readonly IPresenter<UpdateStatGameAction> _StatsPresenter;

        public Dictionary<Stat, int> StatsWithValue { get; }
        public IEnumerable<Stat> AllStats => StatsWithValue.Keys.ToList();

        public override bool CanBeExecuted => StatsWithValue.Count > 0;

        /*******************************************************************/
        public UpdateStatGameAction(Stat stat, int value) : this(new Dictionary<Stat, int> { { stat, value } }) { }

        public UpdateStatGameAction(Dictionary<Stat, int> statsWithValues)
        {
            StatsWithValue = statsWithValues;
        }

        /*******************************************************************/
        public bool HasStat(Stat stat) => StatsWithValue.ContainsKey(stat);

        protected override async Task ExecuteThisLogic()
        {
            _statsWithOldValue = StatsWithValue.ToDictionary(statNewValues => statNewValues.Key, kvp => kvp.Key.Value);
            StatsWithValue.ForEach(statNewValues => statNewValues.Key.UpdateValue(statNewValues.Value));
            await _StatsPresenter.PlayAnimationWith(this);
        }

        public override async Task Undo()
        {
            StatsWithValue.ForEach(statNewValues => statNewValues.Key.UpdateValue(_statsWithOldValue[statNewValues.Key]));
            await _StatsPresenter.PlayAnimationWith(this);
        }
    }
}
