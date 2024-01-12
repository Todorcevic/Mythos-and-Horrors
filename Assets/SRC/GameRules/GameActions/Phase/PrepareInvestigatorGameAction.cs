using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PrepareInvestigatorGameAction : GameAction
    {
        private const int INITIAL_HAND_SIZE = 5;
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
            await _gameActionFactory.Create<MoveCardsGameAction>().Run(_investigator.InvestigatorCard, _investigator.InvestigatorZone);
            _investigator.Cards.ForEach(card => card.IsFaceDown = true);
            await _gameActionFactory.Create<MoveCardsGameAction>().Run(_investigator.Cards, _investigator.DeckZone);

            for (int i = 0; i < INITIAL_HAND_SIZE; i++)
            {
                await _gameActionFactory.Create<InitialDrawGameAction>().Run(_investigator);
            }
        }
    }
}
