using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class GameAction
    {
        private static GameAction _current;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly IPresenter<GameAction> _continuousPresenter;

        public bool IsActive { get; private set; }
        public GameAction Parent { get; private set; }
        protected virtual bool CanBeExecuted => true;

        /*******************************************************************/
        public async Task Start()
        {
            if (!CanBeExecuted) return;
            IsActive = true;
            Parent = _current ?? this;
            _current = this;
            await AtTheBeginning();
            await ExecuteThisLogic();
            await _continuousPresenter.PlayAnimationWith(this);
            await AtTheEnd();
            _current = Parent ?? this;
            IsActive = false;
        }

        private async Task AtTheBeginning() => await _reactionablesProvider.WhenBegin(this);

        protected abstract Task ExecuteThisLogic();

        private async Task AtTheEnd() => await _reactionablesProvider.WhenFinish(this);
    }
}
