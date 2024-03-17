using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class UpdateStatesGameAction : GameAction
    {
        [Inject] private readonly IPresenter<UpdateStatesGameAction> _updateStatesPresenters;
        private readonly Dictionary<State, bool> _statesWithOldValue;

        public IEnumerable<State> States { get; }
        public bool Value { get; }

        /*******************************************************************/
        public UpdateStatesGameAction(State state, bool value) : this(new[] { state }, value) { }

        public UpdateStatesGameAction(IEnumerable<State> states, bool value)
        {
            _statesWithOldValue = states.ToDictionary(kvp => kvp, kvp => kvp.IsActive);
            States = states;
            Value = value;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            States.ForEach(state => state.UpdateValueTo(Value));
            await _updateStatesPresenters.PlayAnimationWith(this);
        }

        public override async Task Undo()
        {
            States.ForEach(state => state.UpdateValueTo(_statesWithOldValue[state]));
            await _updateStatesPresenters.PlayAnimationWith(this);
        }
    }
}
