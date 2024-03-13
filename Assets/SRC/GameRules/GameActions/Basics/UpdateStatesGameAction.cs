using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class UpdateStatesGameAction : GameAction
    {
        public List<State> States { get; }
        public bool Value { get; }

        /*******************************************************************/
        public UpdateStatesGameAction(State state, bool value) : this(new List<State> { state }, value) { }

        public UpdateStatesGameAction(List<State> states, bool value)
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
