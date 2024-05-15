using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class SafeForeach<T> : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Func<IEnumerable<T>> Collection { get; }
        public Func<T, Task> Logic { get; }
        public State Initilized { get; }

        /*******************************************************************/
        public SafeForeach(Func<IEnumerable<T>> collection, Func<T, Task> logic)
        {
            Logic = logic;
            Collection = collection;
            Initilized = new State(false);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Initilized, true));
            List<T> elementsExecuted = new();
            T element = Collection().FirstOrDefault();
            while (element != null && Initilized.IsActive)
            {
                await Logic.Invoke(element);
                elementsExecuted.Add(element);
                element = Collection().Except(elementsExecuted).FirstOrDefault();
            }
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Initilized, false));
        }
    }
}
