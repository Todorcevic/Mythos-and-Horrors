using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PrepareInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly TextsProvider _textsProvider;

        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GameText.PREPARE_INVESTIGATOR_PHASE_NAME;
        public override string Description => _textsProvider.GameText.PREPARE_INVESTIGATOR_PHASE_DESCRIPTION;

        /*******************************************************************/
        public PrepareInvestigatorGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await PositionateInvestigatorCard();
            await ApplyInjuty();
            await ApplyShock();
            await SetTurns();
            await PositionateDeck();
            await CollectResources();
            await DrawInitialHand();
            await Mulligan();
            await DrawInitialHand();
        }

        private async Task PositionateInvestigatorCard()
        {
            await _gameActionFactory.Create(new MoveCardsGameAction(ActiveInvestigator.InvestigatorCard, ActiveInvestigator.InvestigatorZone));
        }

        private async Task ApplyInjuty()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Health, ActiveInvestigator.Injury.Value));
        }

        private async Task ApplyShock()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Sanity, ActiveInvestigator.Shock.Value));
        }

        private async Task SetTurns()
        {
            await _gameActionFactory.Create(new UpdateStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.Turns.MaxValue));
        }

        private async Task PositionateDeck()
        {
            ActiveInvestigator.FullDeck.ForEach(card => card.TurnDown(true));
            await _gameActionFactory.Create(new MoveCardsGameAction(ActiveInvestigator.FullDeck, ActiveInvestigator.DeckZone));
            await _gameActionFactory.Create(new ShuffleGameAction(ActiveInvestigator.DeckZone));
        }

        private async Task CollectResources()
        {
            await _gameActionFactory.Create(new GainResourceGameAction(ActiveInvestigator, 5));
        }

        private async Task DrawInitialHand()
        {
            while (ActiveInvestigator.HandZone.Cards.Count < GameValues.INITIAL_DRAW_SIZE)
                await _gameActionFactory.Create(new InitialDrawGameAction(ActiveInvestigator));
            await Task.Delay(250);
        }

        private async Task Mulligan()
        {
            await _gameActionFactory.Create(new MulliganGameAction(ActiveInvestigator));
        }
    }
}
