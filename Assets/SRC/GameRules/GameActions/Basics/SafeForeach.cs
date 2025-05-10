using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class SafeForeach<T> : GameAction
    {
        public Dictionary<T, State> ElementsExecuted { get; private set; } = new();
        public Func<IEnumerable<T>> Collection { get; private set; }
        public Func<T, Task> Logic { get; private set; }
        public State Initilized { get; private set; }

        /*******************************************************************/
        public SafeForeach<T> SetWith(Func<IEnumerable<T>> collection, Func<T, Task> logic)
        {
            Logic = logic;
            Collection = collection;
            Initilized = new State(false);
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Initilized, true).Execute();
            await ResolveLogic();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Initilized, false).Execute();

            /*******************************************************************/
            async Task ResolveLogic()
            {
                Collection().Except(ElementsExecuted.Keys).ForEach(element => ElementsExecuted.Add(element, new State(false)));
                T element = ElementsExecuted.Keys.FirstOrDefault(element => !ElementsExecuted[element].IsActive);
                if (element == null || !Initilized.IsActive) return;
                if (Collection().Contains(element)) await Logic.Invoke(element);
                await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ElementsExecuted[element], true).Execute();
                await ResolveLogic();
            }
        }
    }
}
