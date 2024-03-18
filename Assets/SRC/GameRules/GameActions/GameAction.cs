using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class GameAction
    {
        private static GameAction _current;
        private Stack<GameAction> _childGameActions = new();
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;

        protected bool UndoInverse { get; set; }
        public bool IsActive { get; private set; }
        public GameAction Parent { get; private set; }
        public bool CanBeExecuted { get; protected set; } = true;

        /*******************************************************************/
        public async Task Start()
        {
            if (!CanBeExecuted) return;
            IsActive = true;
            Parent = _current ?? this;
            _current = this;
            Parent._childGameActions.Push(this);

            await _reactionablesProvider.WheBegin(this);
            await ExecuteThisLogic();
            await _buffsProvider.CheckAllBuffs(this);
            await _reactionablesProvider.WhenFinish(this);

            _current = Parent ?? this;
            IsActive = false;
        }

        protected abstract Task ExecuteThisLogic();

        public virtual async Task Undo()
        {
            if (UndoInverse) Reverse();
            while (_childGameActions.Count > 0) await _childGameActions.Pop().Undo();
        }

        /*******************************************************************/
        private void Reverse()
        {
            Stack<GameAction> tempStack = new();
            while (_childGameActions.Count > 0) tempStack.Push(_childGameActions.Pop());
            _childGameActions = tempStack;
        }
    }
}
