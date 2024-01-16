using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class GameActionFactory
    {
        [Inject] private readonly DiContainer _container;
        public List<GameAction> Actions { get; } = new();

        /*******************************************************************/
        public T Create<T>() where T : GameAction
        {
            T newAction = _container.Instantiate<T>();
            Actions.Add(newAction);
            return newAction;
        }
    }
}
