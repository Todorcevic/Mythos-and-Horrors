using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class InitialDrawGameAction : GameAction
    {
        private Investigator _investigator;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        /*******************************************************************/
        public async Task Run(Investigator investigator)
        {
            _investigator = investigator;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Card nextDraw = _investigator.DeckZone.Cards.Last();
            if (nextDraw is IWeakness)
            {
                await _gameActionFactory.Create<DiscardGameAction>().Run(nextDraw);
                await _gameActionFactory.Create<InitialDrawGameAction>().Run(_investigator);
                return;
            }

            await _gameActionFactory.Create<DrawGameAction>().Run(_investigator);
        }
    }
}
