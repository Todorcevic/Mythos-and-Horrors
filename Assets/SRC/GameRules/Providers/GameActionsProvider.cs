using System;
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

        public T GetRealLastActive<T>() where T : GameAction => _allGameActionsExecuted.OfType<T>().FirstOrDefault(gameAction => gameAction.IsActive);
        public Investigator GetActiveInvestigator() => _allGameActionsExecuted.OfType<PhaseGameAction>().First().ActiveInvestigator;
        public PhaseGameAction GetRealCurrentPhase() => _allGameActionsExecuted.OfType<PhaseGameAction>()
            .First(phaseGameAction => phaseGameAction.IsActive && phaseGameAction is not ChallengePhaseGameAction);

        /*******************************************************************/
        public async Task<T> Create<T>(T gameAction) where T : GameAction
        {
            _container.Inject(gameAction);
            await gameAction.Start();
            return gameAction;
        }

        public void AddUndo(GameAction gameAction) => _allGameActionsExecuted.Push(gameAction);

        public async Task Rewind()
        {
            while (_allGameActionsExecuted.Count > 0) await _allGameActionsExecuted.Pop().Undo();
        }

        public async Task UndoLast()
        {
            if (_allGameActionsExecuted.Count > 0) await _allGameActionsExecuted.Pop().Undo();
        }

        public async Task UndoLastInteractable()
        {
            if (!CanUndo()) throw new Exception("Can't undo last interactable");
            InteractableGameAction lastInteractableToUndo = GetInteractableToUndo();

            while (_allGameActionsExecuted.Count > 0)
            {
                GameAction lastGameAction = _allGameActionsExecuted.Pop();
                await lastGameAction.Undo();
                if (lastGameAction == lastInteractableToUndo) break;
            }
        }

        public bool CanUndo()
        {
            InteractableGameAction lastInteractableToUndo = GetInteractableToUndo();
            if (lastInteractableToUndo?.IsUniqueEffect ?? true) return false;

            foreach (GameAction gameAction in _allGameActionsExecuted)
            {
                if (!gameAction.CanUndo) return false;
                if (gameAction == lastInteractableToUndo) break;
            }

            return true;
        }

        private InteractableGameAction GetInteractableToUndo() =>
             _allGameActionsExecuted.OfType<InteractableGameAction>().Skip(1).FirstOrDefault();
    }
}
