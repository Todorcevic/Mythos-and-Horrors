using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PrepareInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;

        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GameText.PREPARE_INVESTIGATOR_PHASE_NAME;
        public override string Description => _textsProvider.GameText.PREPARE_INVESTIGATOR_PHASE_DESCRIPTION;

        /*******************************************************************/
        public PrepareInvestigatorGameAction SetWith(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await PositionateInvestigatorCard();
            await PositionatePermanentCards();
            await PositionateDeck();
            await CollectResources();
            await DrawInitialHand();
            await Mulligan();
            await DrawInitialHand();
        }

        private async Task PositionateInvestigatorCard()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(ActiveInvestigator.InvestigatorCard, ActiveInvestigator.InvestigatorZone).Start();
        }

        private async Task PositionatePermanentCards()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(ActiveInvestigator.PermanentCards, ActiveInvestigator.AidZone).Start();
        }

        private async Task PositionateDeck()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(ActiveInvestigator.FullDeck, ActiveInvestigator.DeckZone, isFaceDown: true).Start();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(ActiveInvestigator.DeckZone).Start();
        }

        private async Task CollectResources()
        {
            await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(ActiveInvestigator, 5).Start();
        }

        private async Task DrawInitialHand()
        {
            await _gameActionsProvider.Create<InitialDrawGameAction>().SetWith(ActiveInvestigator).Start();
            await Task.Delay(400); //TODO: Remove this delay, its must be in GameView
        }

        private async Task Mulligan()
        {
            await _gameActionsProvider.Create<MulliganGameAction>().SetWith(ActiveInvestigator).Start();
        }
    }
}
