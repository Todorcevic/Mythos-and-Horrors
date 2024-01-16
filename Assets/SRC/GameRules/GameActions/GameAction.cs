using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class GameAction
    {
        [Inject] private readonly IInteractable _UIActivable;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        /*******************************************************************/
        protected async Task Start()
        {
            await _UIActivable.DeactivateAll();
            await AtTheBeginning();
            await ExecuteThisLogic();
            await AtTheEnd();
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
