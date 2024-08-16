using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PrepareInvestigatorGameAction : PhaseGameAction
    {
        public override Phase MainPhase => Phase.Prepare;
        public override string Name => _textsProvider.GetLocalizableText("PhaseName_PrepareInvestigator");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_PrepareInvestigator");

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
            //await Mulligan();
            await DrawInitialHand();
            await RestoreCards();
        }

        private async Task PositionateInvestigatorCard()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(ActiveInvestigator.InvestigatorCard, ActiveInvestigator.InvestigatorZone).Execute();
        }

        private async Task PositionatePermanentCards()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(ActiveInvestigator.PermanentCards, ActiveInvestigator.AidZone).Execute();
        }

        private async Task PositionateDeck()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(ActiveInvestigator.FullDeck, ActiveInvestigator.DeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(ActiveInvestigator.DeckZone).Execute();
        }

        private async Task CollectResources()
        {
            await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(ActiveInvestigator, 5).Execute();
        }

        private async Task DrawInitialHand()
        {
            await _gameActionsProvider.Create<InitialDrawGameAction>().SetWith(ActiveInvestigator).Execute();
        }

        private async Task Mulligan()
        {
            await _gameActionsProvider.Create<MulliganGameAction>().SetWith(ActiveInvestigator).Execute();
        }

        private async Task RestoreCards()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(ActiveInvestigator.DiscardZone.Cards, ActiveInvestigator.DeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(ActiveInvestigator.DeckZone).Execute();
        }
    }
}
