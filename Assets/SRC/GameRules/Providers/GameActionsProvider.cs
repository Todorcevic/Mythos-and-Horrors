using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GameActionsProvider
    {
        private readonly Stack<GameAction> _undablesGameActions = new();

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


        public void AddUndo(GameAction gameAction) => _undablesGameActions.Push(gameAction);

        public async Task UndoRewind()
        {
            while (_undablesGameActions.Count > 0)
            {
                if (_undablesGameActions.Pop() is IUndable undable) await undable.Undo();
            }
        }

        public async Task UndoLast()
        {
            while (_undablesGameActions.Count > 0)
            {
                if (_undablesGameActions.Pop() is IUndable undable)
                {
                    await undable.Undo();
                    break;
                }
            }
        }

        public async Task UndoLastInteractable()
        {
            GameAction returnedGameAction = default;
            while (_undablesGameActions.Count > 0)
            {
                GameAction currentGameAction = _undablesGameActions.Pop();

                if (currentGameAction is IUndable undable)
                {
                    await undable.Undo();
                }
                else if (currentGameAction is InteractableGameAction interactable)
                {
                    returnedGameAction = interactable.Parent;
                    break;
                }
            }

            AllGameActions.Add(returnedGameAction);
            await returnedGameAction.Start();
        }
    }
}
