using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public abstract class GameAction
    {
        [Inject] protected readonly GameActionRepository _gameActionRepository;

        /*******************************************************************/
        public async Task Run()
        {
            AtStart();
            await Execute();
            AtEnd();
        }

        private void AtStart()
        {

        }

        private void AtEnd()
        {

        }

        protected abstract Task Execute();
    }
}
