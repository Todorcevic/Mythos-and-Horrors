using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InitialDrawGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

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
            if (nextDraw is IWeakness)
            {
                await _gameActionFactory.Create(new DiscardGameAction(nextDraw));
                await _gameActionFactory.Create(new InitialDrawGameAction(Investigator));
                return;
            }

            await _gameActionFactory.Create(new DrawGameAction(Investigator));
        }
    }
}
