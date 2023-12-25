using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InitialDrawGameAction : GameAction
    {
        private Adventurer _adventurer;
        [Inject] private readonly GameActionFactory _gameActionRepository;

        /*******************************************************************/
        public async Task Run(Adventurer adventurer)
        {
            _adventurer = adventurer;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            DrawGameAction drawGameAction = _gameActionRepository.Create<DrawGameAction>();
            await drawGameAction.Run(_adventurer);

            if (drawGameAction.CardDrawed is IWeakness)
            {
                await _gameActionRepository.Create<DiscardGameAction>().Run(drawGameAction.CardDrawed);
                await _gameActionRepository.Create<InitialDrawGameAction>().Run(_adventurer);
            }
        }
    }
}
