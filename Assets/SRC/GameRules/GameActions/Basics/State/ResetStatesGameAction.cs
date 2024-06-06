using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class ResetStatesGameAction : UpdateStatesGameAction
    {
        public ResetStatesGameAction(State state) : base(state, state.InitialState) { }
        public ResetStatesGameAction(IEnumerable<State> states) : base(states.ToDictionary(state => state, state => state.InitialState)) { }
    }
}
