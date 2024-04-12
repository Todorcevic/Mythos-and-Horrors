using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PrepareInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GameText.PREPARE_INVESTIGATOR_PHASE_NAME;
        public override string Description => _textsProvider.GameText.PREPARE_INVESTIGATOR_PHASE_DESCRIPTION;

        public override bool CanBeExecuted => ActiveInvestigator != null;

        /*******************************************************************/
        public PrepareInvestigatorGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await PositionateInvestigatorCard();
            await PositionateDeck();
            await CollectResources();
            await DrawInitialHand();
            await Mulligan();
            await DrawInitialHand();
            await _gameActionsProvider.Create(new PrepareInvestigatorGameAction(ActiveInvestigator.NextInvestigator));
        }

        private async Task PositionateInvestigatorCard()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(ActiveInvestigator.InvestigatorCard, ActiveInvestigator.InvestigatorZone));
        }

        private async Task PositionateDeck()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(ActiveInvestigator.FullDeck, ActiveInvestigator.DeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new ShuffleGameAction(ActiveInvestigator.DeckZone));
        }

        private async Task CollectResources()
        {
            await _gameActionsProvider.Create(new GainResourceGameAction(ActiveInvestigator, 5));
        }

        private async Task DrawInitialHand()
        {
            await _gameActionsProvider.Create(new InitialDrawGameAction(ActiveInvestigator));
            await Task.Delay(400); //TODO: Remove this delay, its must be in GameView
        }

        private async Task Mulligan()
        {
            await _gameActionsProvider.Create(new MulliganGameAction(ActiveInvestigator));
        }
    }
}
