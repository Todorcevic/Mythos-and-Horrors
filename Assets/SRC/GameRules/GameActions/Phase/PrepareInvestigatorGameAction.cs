using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PrepareInvestigatorGameAction : GameAction
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
            await PositionateInvestigatorCard();
            await ApplyInjuty();
            await ApplyShock();
            await PositionateDeck();
            await CollectResources();
            await DrawInitialHand();
            await Mulligan();
            await DrawInitialHand();
        }

        private async Task PositionateInvestigatorCard()
        {
            await _gameActionFactory.Create<MoveCardsGameAction>().Run(_investigator.InvestigatorCard, _investigator.InvestigatorZone);
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

        private async Task PositionateDeck()
        {
            _investigator.FullDeck.ForEach(card => card.TurnDown(true));
            await _gameActionFactory.Create<MoveCardsGameAction>().Run(_investigator.FullDeck, _investigator.DeckZone);
            await _gameActionFactory.Create<ShuffleGameAction>().Run(_investigator.DeckZone);
        }

        private async Task CollectResources()
        {
            await _gameActionFactory.Create<GainResourceGameAction>().Run(_investigator, 5);
        }

        private async Task DrawInitialHand()
        {
            while (_investigator.HandZone.Cards.Count < _investigator.InitialHandSize.Value)
                await _gameActionFactory.Create<InitialDrawGameAction>().Run(_investigator);
            await Task.Delay(250);
        }

        private async Task Mulligan()
        {
            await _gameActionFactory.Create<MulliganGameAction>().Run(_investigator);
        }
    }
}
