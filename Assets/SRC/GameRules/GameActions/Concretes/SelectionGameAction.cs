using System.Threading.Tasks;
using Zenject;

namespace GameRules
{
    public class SelectionGameAction : GameAction
    {
        [Inject] private readonly IGameActionSelecter _gameActionSelecter;
        [Inject] private readonly GameActionFactory _gameActionRepository;
        private GameAction[] _gameActions;

        /*******************************************************************/
        public SelectionGameAction Set(params GameAction[] gameActions)
        {
            _gameActions = gameActions;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionSelecter.ShowThisActions(_gameActions);
            await _gameActionRepository.Create<WaitingForSelectionGameAction>().Run();
        }
    }
}
