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
            .First(phaseGameAction => phaseGameAction.IsActive && phaseGameAction is not ChallengePhaseGameAction);

        public ChallengePhaseGameAction CurrentChallenge => GetRealLastActive<ChallengePhaseGameAction>();
        public InteractableGameAction CurrentInteractable => GetRealLastActive<InteractableGameAction>();
        public GameAction CurrentGameAction => _allGameActionsExecuted.Peek();

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

        public async Task UndoUntil(GameAction gameAction)
        {
            while (_allGameActionsExecuted.Count > 0)
            {
                GameAction lastGameAction = _allGameActionsExecuted.Pop();
                await lastGameAction.Undo();
                if (lastGameAction == gameAction) break;
            }
        }

        public async Task<InteractableGameAction> UndoLastInteractable()
        {
            if (!CanUndo()) throw new Exception("Can't undo last interactable");
            InteractableGameAction lastInteractableToUndo = GetInteractableToUndo();

            while (_allGameActionsExecuted.Count > 0)
            {
                GameAction lastGameAction = _allGameActionsExecuted.Pop();
                await lastGameAction.Undo();
                if (lastGameAction == lastInteractableToUndo) break;
            }
            return lastInteractableToUndo;
        }

        public bool CanUndo()
        {
            InteractableGameAction lastInteractableToUndo = GetInteractableToUndo();
            if (lastInteractableToUndo == null) return false;
            if (lastInteractableToUndo.GetUniqueEffect() != null) return false;

            foreach (GameAction gameAction in _allGameActionsExecuted)
            {
                if (!gameAction.CanUndo) return false;
                if (gameAction == lastInteractableToUndo) break;
            }

            return true;
        }

        private InteractableGameAction GetInteractableToUndo() =>
             _allGameActionsExecuted.OfType<InteractableGameAction>().Skip(1)
            .Where(interactableGameAction => interactableGameAction.CanBackToThisInteractable)
            .FirstOrDefault();
    }
}
