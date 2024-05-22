using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class UpdateStatesGameAction : GameAction
    {
        private Dictionary<State, bool> _statesWithOldValue;
        [Inject] private readonly IPresenter<UpdateStatesGameAction> _updateStatesPresenters;

        public Dictionary<State, bool> StatesDictionary { get; }
        public List<State> States => StatesDictionary.Keys.ToList();

        /*******************************************************************/
        public UpdateStatesGameAction(State state, bool value) : this(new Dictionary<State, bool> { { state, value } })
        { }

        public UpdateStatesGameAction(IEnumerable<State> states, bool value) : this(states.ToDictionary(state => state, state => value))
        { }

        public UpdateStatesGameAction(Dictionary<State, bool> states)
        {
            StatesDictionary = states.Where(kvp => kvp.Key.IsActive != kvp.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _statesWithOldValue = StatesDictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Key.IsActive);
            StatesDictionary.ForEach(kvp => kvp.Key.UpdateValueTo(kvp.Value));
            await _updateStatesPresenters.PlayAnimationWith(this);
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            StatesDictionary.ForEach(kvp => kvp.Key.UpdateValueTo(_statesWithOldValue[kvp.Key]));
            await base.Undo();
            await _updateStatesPresenters.PlayAnimationWith(this);
        }
    }
}
