using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{

    public class DrawGameAction : GameAction
    {
        private Adventurer _adventurer;
        [Inject] private readonly GameActionFactory _gameActionRepository;

        public Card CardDrawed { get; private set; }

        /*******************************************************************/
        public async Task<Card> Run(Adventurer adventurer)
        {
            _adventurer = adventurer;
            await Start();
            return CardDrawed;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CardDrawed = _adventurer.DeckZone.Cards.Last();
            await _gameActionRepository.Create<MoveCardGameAction>().Run(CardDrawed, _adventurer.HandZone);
        }
    }
}
