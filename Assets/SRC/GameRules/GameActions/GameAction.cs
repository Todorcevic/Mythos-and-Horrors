using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class GameAction
    {
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly IUIActivator _activatorUIPresenter;

        /*******************************************************************/
        protected async Task Start()
        {
            _activatorUIPresenter.Deactivate();
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
