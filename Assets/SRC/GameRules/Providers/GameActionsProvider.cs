using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GameActionsProvider
    {
        [Inject] private readonly DiContainer _container;
        public List<GameAction> AllGameActions { get; } = new();
        public List<GameAction> GameActionsFinished => AllGameActions.FindAll(gameAction => !gameAction.IsActive);

        /*******************************************************************/
        public async Task<T> Create<T>(T gameAction) where T : GameAction
        {
            _container.Inject(gameAction);
            AllGameActions.Add(gameAction);
            await gameAction.Start();
            return gameAction;
        }

        public T GetLastActive<T>() where T : GameAction =>
            AllGameActions.LastOrDefault(gameAction => gameAction is T && gameAction.IsActive) as T;

    }
}
