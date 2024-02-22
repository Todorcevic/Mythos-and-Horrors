using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class GameAction
    {
        private static GameAction _current;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly INewPresenter<GameAction> _cardStatePresenter;

        public bool IsActive { get; private set; }
        public GameAction Parent { get; private set; }

        /*******************************************************************/
        public async Task Start()
        {
            IsActive = true;
            Parent = _current ?? this;
            _current = this;
            await AtTheBeginning();
            await ExecuteThisLogic();
            await _cardStatePresenter.PlayAnimationWith(this);
            await AtTheEnd();
            _current = Parent ?? this;
            IsActive = false;
        }

        private async Task AtTheBeginning() => await _reactionablesProvider.WhenBegin(this);

        protected abstract Task ExecuteThisLogic();

        private async Task AtTheEnd() => await _reactionablesProvider.WhenFinish(this);
    }
}
