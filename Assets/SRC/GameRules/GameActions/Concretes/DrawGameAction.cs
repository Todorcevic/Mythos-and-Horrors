using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;

        public Adventurer Adventurer { get; private set; }
        public Card CardDrawed { get; private set; }

        /*******************************************************************/
        public async Task Run(Adventurer adventurer)
        {
            Adventurer = adventurer;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CardDrawed = Adventurer.DeckZone.Cards.Last();
            await _gameActionRepository.Create<MoveCardsGameAction>().Run(CardDrawed, Adventurer.HandZone);
        }
    }
}
