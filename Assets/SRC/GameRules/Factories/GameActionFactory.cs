using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class GameActionFactory
    {
        [Inject] private readonly DiContainer _container;
        public List<GameAction> Actions { get; } = new();

        /*******************************************************************/
        public async Task<T> Create<T>(T gameAction) where T : GameAction
        {
            _container.Inject(gameAction);
            Actions.Add(gameAction);
            await gameAction.Start();
            return gameAction;
        }


        public T Create<T>() where T : GameAction
        {
            T newAction = _container.Instantiate<T>();
            Actions.Add(newAction);
            return newAction;
        }
    }
}
