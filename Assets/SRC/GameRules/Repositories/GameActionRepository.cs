using System.Collections.Generic;
using Zenject;

namespace GameRules
{
    public class GameActionRepository
    {
        private readonly DiContainer _container;
        private List<GameAction> _actions = new();

        /*******************************************************************/
        public GameActionRepository(DiContainer container)
        {
            _container = container;
        }

        /*******************************************************************/
        public T GiveMe<T>() where T : GameAction
        {
            T newAction = _container.Instantiate<T>();
            _actions.Add(newAction);
            return newAction;
        }
    }
}
