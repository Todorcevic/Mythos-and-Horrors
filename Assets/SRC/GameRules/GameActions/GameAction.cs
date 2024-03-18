using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class GameAction
    {
        private static GameAction _current;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

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

            await _reactionablesProvider.WhenBegin(this);
            await ExecuteThisLogic();

            _gameActionsProvider.AddUndo(this);

            await _buffsProvider.CheckAllBuffs(this);
            await _reactionablesProvider.WhenFinish(this);

            _current = Parent ?? this;
            IsActive = false;
        }

        protected abstract Task ExecuteThisLogic();

        public virtual async Task Undo() => await Task.CompletedTask;

    }
}
