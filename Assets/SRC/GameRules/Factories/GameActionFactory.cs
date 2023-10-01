using System.Collections.Generic;
using Zenject;

namespace Tuesday.GameRules
{
    public class GameActionFactory
    {
        [Inject] private readonly DiContainer _container;
        private readonly List<GameAction> _actions = new();

        /*******************************************************************/
        public T Create<T>() where T : GameAction
        {
            T newAction = _container.Instantiate<T>();
            _actions.Add(newAction);
            return newAction;
        }
    }
}
