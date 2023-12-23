using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{

    public class DrawGameAction : GameAction
    {
        private Adventurer _adventurer;
        private Card _cardDrawed;
        [Inject] private readonly GameActionFactory _gameActionRepository;

        /*******************************************************************/
        public async Task<Card> Run(Adventurer adventurer)
        {
            _adventurer = adventurer;
            await Start();
            return _cardDrawed;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _cardDrawed = _adventurer.DeckZone.Cards.Last();
            await _gameActionRepository.Create<MoveCardGameAction>().Run(_cardDrawed, _adventurer.HandZone);
        }
    }
}
