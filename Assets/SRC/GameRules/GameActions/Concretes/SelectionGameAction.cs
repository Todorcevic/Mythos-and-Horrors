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
        public async Task Start(params GameAction[] gameActions)
        {
            _gameActions = gameActions;
            await Run();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionSelecter.ShowThisActions(_gameActions);
            await _gameActionRepository.Create<WaitingForSelectionGameAction>().Start();
        }
    }
}
