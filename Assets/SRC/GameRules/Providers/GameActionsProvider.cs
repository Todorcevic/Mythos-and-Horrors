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

        public async Task Rewind()
        {
            while (_allGameActionsExecuted.Count > 0)
            {
                await _allGameActionsExecuted.Pop().Undo();
            }

            await AllGameActionsCreated.First().Start();
        }

        public async Task<PlayInvestigatorGameAction> UndoLast()
        {
            int i = 0;
            while (_allGameActionsExecuted.Count > 0)
            {
                GameAction lastGameAction = _allGameActionsExecuted.Pop();
                await lastGameAction.Undo();
                if (lastGameAction is PlayInvestigatorGameAction lastPlayInvestigator)
                {
                    if (i < 1)
                    {
                        i++;
                        continue;
                    }
                    return lastPlayInvestigator;
                }
            }
            return null;
        }
    }
}
