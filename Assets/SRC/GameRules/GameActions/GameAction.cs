using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class GameAction
    {
        private static GameAction _current;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly OptativeReactionsProvider _optativeReactionsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;
        [Inject] protected readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly IPresenterAnimation _presenterAnimation;

        public GameAction Parent { get; } = _current;
        public bool IsActive { get; private set; }
        public bool IsCancel { get; protected set; }
        public virtual bool CanBeExecuted => true;
        public virtual bool CanUndo => true;

        /*******************************************************************/
        public async Task Execute()
        {
            if (Parent?.IsCancel ?? false) Cancel();
            await _reactionablesProvider.WhenInitial(this);
            if (!CanBeExecuted || IsCancel) return;
            InitialSet();
            await _reactionablesProvider.WhenBegin(this);
            await _optativeReactionsProvider.WhenBegin(this);
            if (!CanBeExecuted || IsCancel)
            {
                FinishSet();
                return;
            }
            _gameActionsProvider.AddUndo(this);

            await _presenterAnimation.PlayBeforeAnimationWith(this);
            await ExecuteThisLogic();

            await _presenterAnimation.PlayAfterAnimationWith(this);
            await _buffsProvider.ExecuteAllBuffs();
            await _reactionablesProvider.WhenFinish(this);
            await _optativeReactionsProvider.WhenFinish(this);
            FinishSet();
        }

        public virtual async Task Undo()
        {
            _buffsProvider.UndoAllBuffs();
            await _presenterAnimation.PlayUndoAnimationWith(this);
        }

        public void Cancel() => IsCancel = true;

        protected abstract Task ExecuteThisLogic();

        private void InitialSet()
        {
            IsActive = true;
            _current = this;
        }

        private void FinishSet()
        {
            _current = Parent ?? this;
            IsActive = false;
        }
    }
}
