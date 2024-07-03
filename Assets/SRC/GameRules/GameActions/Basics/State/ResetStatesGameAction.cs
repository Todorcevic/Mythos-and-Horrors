using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class ResetStatesGameAction : UpdateStatesGameAction
    {
        public ResetStatesGameAction SetWith(State state)
        {
            SetWith(state, state.InitialState);
            return this;
        }

        public ResetStatesGameAction SetWith(IEnumerable<State> states)
        {
            SetWith(states.ToDictionary(state => state, state => state.InitialState));
            return this;
        }
    }
}