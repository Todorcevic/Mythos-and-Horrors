using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class SafeForeach<T> : GameAction
    {
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
            List<T> elementsExecuted = new();
            T element = Collection().FirstOrDefault();
            await ResolveLogic();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Initilized, false).Execute();

            /*******************************************************************/
            async Task ResolveLogic()
            {
                if (element == null || !Initilized.IsActive) return;
                await Logic.Invoke(element);
                elementsExecuted.Add(element);
                element = Collection().Except(elementsExecuted).FirstOrDefault();
                await ResolveLogic();
            }
        }
    }
}
