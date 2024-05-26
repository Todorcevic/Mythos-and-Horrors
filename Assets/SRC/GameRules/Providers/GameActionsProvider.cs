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

        private T GetRealLastActive<T>() where T : GameAction => _allGameActionsExecuted.OfType<T>().FirstOrDefault(gameAction => gameAction.IsActive);
        public PhaseGameAction GetRealCurrentPhase() => _allGameActionsExecuted.OfType<PhaseGameAction>()
            .FirstOrDefault(phaseGameAction => phaseGameAction.IsActive && phaseGameAction is not ChallengePhaseGameAction);

        public ChallengePhaseGameAction CurrentChallenge => GetRealLastActive<ChallengePhaseGameAction>();
        public InteractableGameAction CurrentInteractable => GetRealLastActive<InteractableGameAction>();

        /*******************************************************************/
        public async Task<T> Create<T>(T gameAction) where T : GameAction
        {
            _container.Inject(gameAction);
            if (gameAction is IInitializable initializable) initializable.ExecuteSpecificInitialization();
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

        public async Task UndoUntil(GameAction gameAction)
        {
            while (_allGameActionsExecuted.Count > 0)
            {
                GameAction lastGameAction = _allGameActionsExecuted.Pop();
                await lastGameAction.Undo();
                if (lastGameAction == gameAction) break;
            }
        }

        public async Task<InteractableGameAction> CancelInteractable()
        {
            if (!CanUndo(realLast: true)) throw new Exception("Can't undo last interactable");
            InteractableGameAction lastInteractableToUndo = GetInteractableToUndo(realLast: true);

            await UndoUntil(lastInteractableToUndo);
            return lastInteractableToUndo;
        }

        public async Task<InteractableGameAction> UndoLastInteractable()
        {
            if (!CanUndo()) throw new Exception("Can't undo last interactable");
            InteractableGameAction lastInteractableToUndo = GetInteractableToUndo();

            await UndoUntil(lastInteractableToUndo);
            return lastInteractableToUndo;
        }

        public bool CanUndo(bool realLast = false)
        {
            InteractableGameAction lastInteractableToUndo = GetInteractableToUndo(realLast);
            if (lastInteractableToUndo == null) return false;
            if (lastInteractableToUndo.GetUniqueEffect() != null) return false;

            foreach (GameAction gameAction in _allGameActionsExecuted)
            {
                if (!gameAction.CanUndo) return false;
                if (gameAction == lastInteractableToUndo) break;
            }

            return true;
        }

        public InteractableGameAction GetInteractableToUndo(bool realLast = false) =>
             _allGameActionsExecuted.OfType<InteractableGameAction>().Skip(realLast ? 0 : 1)
            .Where(interactableGameAction => interactableGameAction.CanBackToThisInteractable)
            .FirstOrDefault();
    }
}
