
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class IncrementStatGameAction : GameAction
    {
        public Dictionary<Stat, int> StatsWithValue { get; private set; }

        /*******************************************************************/
        public IncrementStatGameAction SetWith(Stat stat, int value) => SetWith(new Dictionary<Stat, int> { { stat, value } });

        public IncrementStatGameAction SetWith(Dictionary<Stat, int> statsWithValues)
        {
            StatsWithValue = statsWithValues;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>()
                .SetWith(StatsWithValue.ToDictionary(d => d.Key, d => d.Key.RealValue + d.Value))
                .Execute();
        }
    }
}
