using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
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
            CardDrawed = Investigator.CardToDraw;
            CardDrawed.TurnDown(false);
            await _gameActionRepository.Create(new MoveCardsGameAction(CardDrawed, Investigator.HandZone));
        }
    }
}
