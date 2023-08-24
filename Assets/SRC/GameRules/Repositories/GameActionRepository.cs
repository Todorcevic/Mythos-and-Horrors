using System.Collections.Generic;
using Zenject;

namespace GameRules
{
    public class GameActionRepository
    {
        [Inject] private readonly DiContainer _container;
        private List<GameAction> _actions = new();

        /*******************************************************************/
        public T Create<T>() where T : GameAction
        {
            T newAction = _container.Instantiate<T>();
            _actions.Add(newAction);
            return newAction;
        }
    }
}
