using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResetStatGameAction : GameAction
    {
        public IEnumerable<Stat> Stats { get; private set; }

        /*******************************************************************/
        public ResetStatGameAction SetWith(Stat stat) => SetWith(new[] { stat });

        public ResetStatGameAction SetWith(IEnumerable<Stat> stats)
        {
            Stats = stats;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>()
                .SetWith(Stats.ToDictionary(stat => stat, stat => stat.InitialValue))
                .Execute();
        }
    }
}
