using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PrepareAdventurerGameAction : GameAction
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
            await _gameActionRepository.Create<MoveCardGameAction>().Run(_adventurer.AdventurerCard, _adventurer.AdventurerZone);
            await _gameActionRepository.Create<MoveCardsGameAction>().Run(_adventurer.Cards.ToArray(), _adventurer.DeckZone);

            await _gameActionRepository.Create<DrawGameAction>().Run(_adventurer);
            await _gameActionRepository.Create<DrawGameAction>().Run(_adventurer);
            await _gameActionRepository.Create<DrawGameAction>().Run(_adventurer);
            await _gameActionRepository.Create<DrawGameAction>().Run(_adventurer);
            await _gameActionRepository.Create<DrawGameAction>().Run(_adventurer);
        }
    }
}
