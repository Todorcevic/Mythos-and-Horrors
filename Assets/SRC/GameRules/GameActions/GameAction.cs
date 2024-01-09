using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class GameAction
    {
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly AnimatorsProvider _animatorsProvider;

        /*******************************************************************/
        protected async Task Start()
        {
            await AtTheBeginning();
            await ExecuteThisLogic();
            await AtTheEnd();
        }

        private async Task AtTheBeginning()
        {
            await _reactionablesProvider.WhenBegin(this);
            await _animatorsProvider.LaunchStartAnimation(this);
        }

        protected abstract Task ExecuteThisLogic();

        private async Task AtTheEnd()
        {
            await _animatorsProvider.LaunchEndAnimation(this);
            await _reactionablesProvider.WhenFinish(this);
        }
    }
}
