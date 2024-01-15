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

            await ApplyInjuty();
            await ApplyShock();

            _investigator.Cards.ForEach(card => card.IsFaceDown = true);
            await _gameActionFactory.Create<MoveCardsGameAction>().Run(_investigator.Cards, _investigator.DeckZone);

            for (int i = 0; i < INITIAL_HAND_SIZE; i++)
            {
                await _gameActionFactory.Create<InitialDrawGameAction>().Run(_investigator);
            }
        }

        private async Task ApplyInjuty()
        {
            await _gameActionFactory.Create<DecrementStatGameAction>()
                .Run(_investigator.InvestigatorCard.Health, _investigator.Injury.Value);
        }

        private async Task ApplyShock()
        {
            await _gameActionFactory.Create<DecrementStatGameAction>()
                .Run(_investigator.InvestigatorCard.Sanity, _investigator.Shock.Value);
        }
    }
}
