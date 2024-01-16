using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;

        public Investigator Investigator { get; private set; }
        public Card CardDrawed { get; private set; }

        /*******************************************************************/
        public async Task<Card> Run(Investigator investigator)
        {
            Investigator = investigator;
            await Start();
            return CardDrawed;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CardDrawed = Investigator.DeckZone.Cards.Last();
            CardDrawed.IsFaceDown = false;
            await _gameActionRepository.Create<MoveCardsGameAction>().Run(CardDrawed, Investigator.HandZone);
        }
    }
}
