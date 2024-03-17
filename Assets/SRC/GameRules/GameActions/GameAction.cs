using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class GameAction
    {
        private static GameAction _current;
        private readonly Stack<GameAction> _gameActionToUndo = new();
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;

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
            Parent._gameActionToUndo.Push(this);

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
            while (_gameActionToUndo.Count > 0) await _gameActionToUndo.Pop().Undo();
        }
    }
}
