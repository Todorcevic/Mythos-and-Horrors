using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class GameActionsProvider
    {
        [Inject] private readonly DiContainer _container;
        private readonly Stack<GameAction> _allGameActionsExecuted = new();
        private readonly List<GameAction> _allGameActionsCreated = new();

        public T GetLastCreate<T>() where T : GameAction => _allGameActionsCreated.OfType<T>().LastOrDefault(gameAction => gameAction.IsActive);
        public T GetRealLastActive<T>() where T : GameAction => _allGameActionsExecuted.OfType<T>().FirstOrDefault(gameAction => gameAction.IsActive);

        public Investigator GetActiveInvestigator() => _allGameActionsExecuted.OfType<PhaseGameAction>().First()?.ActiveInvestigator;

        /*******************************************************************/
        public async Task<T> Create<T>(T gameAction) where T : GameAction
        {
            _container.Inject(gameAction);
            _allGameActionsCreated.Add(gameAction);
            await gameAction.Start();
            return gameAction;
        }

        public void AddUndo(GameAction gameAction) => _allGameActionsExecuted.Push(gameAction);

        public async Task Rewind()
        {
            while (_allGameActionsExecuted.Count > 0) await _allGameActionsExecuted.Pop().Undo();
            await _allGameActionsCreated.First().Start();
        }

        public async Task<InteractableGameAction> UndoLastInteractable()
        {
            int i = 0;
            while (_allGameActionsExecuted.Count > 0)
            {
                GameAction lastGameAction = _allGameActionsExecuted.Pop();
                await lastGameAction.Undo();
                if (lastGameAction is InteractableGameAction lastPlayInvestigator)
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
