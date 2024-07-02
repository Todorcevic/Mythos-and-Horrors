using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResetStatesGameAction : GameAction
    {
        public IEnumerable<State> States { get; private set; }

        /*******************************************************************/
        public ResetStatesGameAction SetWith(State state) => SetWith(new[] { state });

        public ResetStatesGameAction SetWith(IEnumerable<State> states)
        {
            States = states;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>()
                .SetWith(States.ToDictionary(state => state, state => state.InitialState))
                .Execute();
        }
    }
}