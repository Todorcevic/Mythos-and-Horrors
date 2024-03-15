using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PrepareInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
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
            await ApplyStats();
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

        private async Task ApplyStats()
        {
            Dictionary<Stat, int> stats = new()
            {
                { ActiveInvestigator.Health, ActiveInvestigator.Health.Value - ActiveInvestigator.Injury.Value },
                { ActiveInvestigator.Sanity,  ActiveInvestigator.Sanity.Value - ActiveInvestigator.Shock.Value },
                { ActiveInvestigator.CurrentTurns, ActiveInvestigator.MaxTurns.Value }
            };

            await _gameActionFactory.Create(new UpdateStatGameAction(stats));
        }

        private async Task PositionateDeck()
        {
            await _gameActionFactory.Create(new UpdateStatesGameAction(ActiveInvestigator.FullDeck.Select(card => card.FaceDown).ToList(), true));
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
