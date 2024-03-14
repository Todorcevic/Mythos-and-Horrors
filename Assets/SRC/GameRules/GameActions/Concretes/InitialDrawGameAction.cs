using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InitialDrawGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public InitialDrawGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card nextDraw = Investigator.DeckZone.Cards.Last();
            if (nextDraw is IFlaw)
            {
                await _gameActionFactory.Create(new DiscardGameAction(nextDraw));
                await _gameActionFactory.Create(new InitialDrawGameAction(Investigator));
                return;
            }

            await _gameActionFactory.Create(new DrawAidGameAction(Investigator));
        }
    }
}
