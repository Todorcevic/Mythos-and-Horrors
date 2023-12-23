using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PrepareAdventurerGameAction : GameAction
    {
        private const int INITIAL_HAND_SIZE = 5;
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
            await _gameActionRepository.Create<MoveCardsGameAction>().Run(_adventurer.Cards, _adventurer.DeckZone);

            for (int i = 0; i < INITIAL_HAND_SIZE; i++)
            {
                await MulliganDraw();
            }
        }

        private async Task MulliganDraw()
        {
            Card cardDrawed = await _gameActionRepository.Create<DrawGameAction>().Run(_adventurer);
            if (cardDrawed.IsWeakenessCard)
            {
                await _gameActionRepository.Create<MoveCardGameAction>().Run(cardDrawed, _adventurer.DiscardZone);
                await MulliganDraw();
            }
        }

    }
}
