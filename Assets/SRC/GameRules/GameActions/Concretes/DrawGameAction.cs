using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionRepository;

        public Investigator Investigator { get; }
        public Card CardDrawed { get; private set; }

        /*******************************************************************/
        public DrawGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CardDrawed = Investigator.DeckZone.Cards.Last();
            await _gameActionRepository.Create(new TurnCardGameAction(CardDrawed, toFaceDown: false));
            await _gameActionRepository.Create(new MoveCardsGameAction(CardDrawed, Investigator.HandZone));
        }
    }
}
