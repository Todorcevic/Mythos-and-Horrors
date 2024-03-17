using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckMaxHandSizeGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly EffectsProvider _effectProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CheckMaxHandSizeGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CheckMaxHandSizeGameAction);
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        public CheckMaxHandSizeGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            while (ActiveInvestigator.HandSize > GameValues.MAX_HAND_SIZE)
            {
                Create();
                await _gameActionsProvider.Create(new InteractableGameAction());
            }
        }

        private void Create()
        {
            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                if (!CanChoose()) continue;

                _effectProvider.Create()
                    .SetCard(card)
                    .SetInvestigator(ActiveInvestigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Discard))
                    .SetLogic(Discard);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create(new DiscardGameAction(card));
                    await _gameActionsProvider.Create(new CheckMaxHandSizeGameAction(ActiveInvestigator));
                };

                bool CanChoose()
                {
                    if (card is IFlaw) return false;
                    return true;
                }
            }
        }


    }
}
