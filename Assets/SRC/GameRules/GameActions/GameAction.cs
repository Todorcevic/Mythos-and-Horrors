using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public abstract class GameAction
    {
        [Inject] private readonly IEnumerable<IStartReactionable> _startReactionables;
        [Inject] private readonly IEnumerable<IEndReactionable> _endReactionables;

        /*******************************************************************/
        protected async Task Start()
        {
            await AtTheBeginning();
            await ExecuteThisLogic();
            await AtTheEnd();
        }

        private async Task AtTheBeginning()
        {
            foreach (IStartReactionable reaction in _startReactionables)
            {
                await reaction.WhenBegin(this);
            }
        }

        protected abstract Task ExecuteThisLogic();

        private async Task AtTheEnd()
        {
            foreach (IEndReactionable reaction in _endReactionables)
            {
                await reaction.WhenFinish(this);
            }
        }
    }
}
