using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PrepareInvestigatorGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public PrepareInvestigatorGameAction(Investigator investigator)
        {
            Investigator = investigator;
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
            await _gameActionFactory.Create(new MoveCardsGameAction(Investigator.InvestigatorCard, Investigator.InvestigatorZone));
        }

        private async Task ApplyInjuty()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.InvestigatorCard.Health, Investigator.InvestigatorCard.Injury.Value));

        }

        private async Task ApplyShock()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.InvestigatorCard.Sanity, Investigator.InvestigatorCard.Shock.Value));
        }

        private async Task PositionateDeck()
        {
            Investigator.FullDeck.ForEach(card => card.TurnDown(true));
            await _gameActionFactory.Create(new MoveCardsGameAction(Investigator.FullDeck, Investigator.DeckZone));
            await _gameActionFactory.Create(new ShuffleGameAction(Investigator.DeckZone));
        }

        private async Task CollectResources()
        {
            await _gameActionFactory.Create(new GainResourceGameAction(Investigator, 2));
        }

        private async Task DrawInitialHand()
        {
            while (Investigator.HandZone.Cards.Count < Investigator.InvestigatorCard.InitialHandSize.Value)
                await _gameActionFactory.Create(new InitialDrawGameAction(Investigator));
            await Task.Delay(250);
        }

        private async Task Mulligan()
        {
            await _gameActionFactory.Create(new MulliganGameAction(Investigator));
        }
    }
}
