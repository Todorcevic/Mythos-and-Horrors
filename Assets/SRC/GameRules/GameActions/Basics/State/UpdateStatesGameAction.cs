using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class UpdateStatesGameAction : GameAction
    {
        private Dictionary<State, bool> _statesWithOldValue;

        public Dictionary<State, bool> StatesDictionary { get; private set; }
        public List<State> States => StatesDictionary.Keys.ToList();

        /*******************************************************************/
        public UpdateStatesGameAction SetWith(State state, bool value) => SetWith(new Dictionary<State, bool> { { state, value } });

        public UpdateStatesGameAction SetWith(IEnumerable<State> states, bool value) =>
            SetWith(states.ToDictionary(state => state, state => value));

        public UpdateStatesGameAction SetWith(Dictionary<State, bool> states)
        {
            StatesDictionary = states.Where(kvp => kvp.Key.IsActive != kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _statesWithOldValue = StatesDictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Key.IsActive);
            StatesDictionary.ForEach(kvp => kvp.Key.UpdateValueTo(kvp.Value));
            await Task.CompletedTask;
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            StatesDictionary.ForEach(kvp => kvp.Key.UpdateValueTo(_statesWithOldValue[kvp.Key]));
            await base.Undo();
        }
    }
}
