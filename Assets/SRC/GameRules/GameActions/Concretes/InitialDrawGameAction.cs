using System.Linq;
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
            Card nextDraw = _adventurer.DeckZone.Cards.Last();
            if (nextDraw is IWeakness)
            {
                await _gameActionRepository.Create<DiscardGameAction>().Run(nextDraw);
                await _gameActionRepository.Create<InitialDrawGameAction>().Run(_adventurer);
                return;
            }

            await _gameActionRepository.Create<DrawGameAction>().Run(_adventurer);
        }
    }
}
