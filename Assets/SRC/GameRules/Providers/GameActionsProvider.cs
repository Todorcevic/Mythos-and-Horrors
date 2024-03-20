using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GameActionsProvider
    {
        private readonly Stack<GameAction> _allGameActionsExecuted = new();

        [Inject] private readonly DiContainer _container;
        public List<GameAction> AllGameActionsCreated { get; } = new();
        public List<GameAction> GameActionsFinished => AllGameActionsCreated.FindAll(gameAction => !gameAction.IsActive);
        public InteractableGameAction LastInteractable => AllGameActionsCreated.OfType<InteractableGameAction>().Last();

        /*******************************************************************/
        public async Task<T> Create<T>(T gameAction) where T : GameAction
        {
            _container.Inject(gameAction);
            AllGameActionsCreated.Add(gameAction);
            await gameAction.Start();
            return gameAction;
        }

        public T GetLastActive<T>() where T : GameAction =>
            AllGameActionsCreated.LastOrDefault(gameAction => gameAction is T && gameAction.IsActive) as T;


        public void AddUndo(GameAction gameAction) => _allGameActionsExecuted.Push(gameAction);

        public async Task UndoRewind()
        {
            while (_allGameActionsExecuted.Count > 0)
            {
                if (_allGameActionsExecuted.Pop() is IUndable undable) await undable.Undo();
            }
        }

        public async Task UndoLast()
        {
            while (_allGameActionsExecuted.Count > 0)
            {
                if (_allGameActionsExecuted.Pop() is IUndable undable)
                {
                    await undable.Undo();
                    break;
                }
            }
        }

        public async Task UndoLastInteractable()
        {
            GameAction returnedGameAction = default;
            while (_allGameActionsExecuted.Count > 0)
            {
                GameAction currentGameAction = _allGameActionsExecuted.Pop();

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

            AllGameActionsCreated.Add(returnedGameAction);
            await returnedGameAction.Start();
        }
    }
}
