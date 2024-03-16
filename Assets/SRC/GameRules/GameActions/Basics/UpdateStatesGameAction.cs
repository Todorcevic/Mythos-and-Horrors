using Sirenix.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class UpdateStatesGameAction : GameAction
    {
        public IEnumerable<State> States { get; }
        public bool Value { get; }

        /*******************************************************************/
        public UpdateStatesGameAction(State state, bool value) : this(new[] { state }, value) { }

        public UpdateStatesGameAction(IEnumerable<State> states, bool value)
        {
            States = states;
            Value = value;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            States.ForEach(state => state.UpdateValueTo(Value));
            await Task.CompletedTask;
        }
    }
}
