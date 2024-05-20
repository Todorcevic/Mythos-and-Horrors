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
        public bool IsCancel { get; private set; }
        public GameAction Parent { get; private set; }
        public virtual bool CanBeExecuted => true;
        public virtual bool CanUndo => true;

        /*******************************************************************/
        public async Task Start()
        {
            if (!CanBeExecuted || IsCancel) return;
            InitialSet();
            await _reactionablesProvider.WhenBegin(this);
            if (!CanBeExecuted || IsCancel)
            {
                FinishSet();
                return;
            }
            _gameActionsProvider.AddUndo(this);

            await ExecuteThisLogic();

            await _buffsProvider.ExecuteAllBuffs();
            await _reactionablesProvider.WhenFinish(this);
            FinishSet();
        }

        public virtual async Task Undo() => await Task.CompletedTask;

        public void Cancel() => IsCancel = true;

        protected abstract Task ExecuteThisLogic();

        private void InitialSet()
        {
            IsActive = true;
            Parent = _current ?? this;
            _current = this;
        }

        private void FinishSet()
        {
            _current = Parent;
            IsActive = false;
        }
    }
}
