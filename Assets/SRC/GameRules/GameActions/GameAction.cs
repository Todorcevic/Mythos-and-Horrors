using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class GameAction
    {
        private static GameAction _current;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public GameAction Parent { get; private set; }

        /*******************************************************************/
        protected async Task Start()
        {
            Parent = _current ?? this;
            _current = this;
            await AtTheBeginning();
            await ExecuteThisLogic();
            await AtTheEnd();
            _current = Parent ?? this;
        }

        private async Task AtTheBeginning()
        {
            await _reactionablesProvider.WhenBegin(this);
        }

        protected abstract Task ExecuteThisLogic();

        private async Task AtTheEnd()
        {
            await _reactionablesProvider.WhenFinish(this);
        }
    }
}
