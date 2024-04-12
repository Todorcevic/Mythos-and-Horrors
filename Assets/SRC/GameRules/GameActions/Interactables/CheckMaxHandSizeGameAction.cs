using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckMaxHandSizeGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CheckMaxHandSizeGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CheckMaxHandSizeGameAction);
        public override Phase MainPhase => Phase.Restore;

        public override bool CanBeExecuted => ActiveInvestigator.HandSize > ActiveInvestigator.MaxHandSize.Value;

        /*******************************************************************/
        public CheckMaxHandSizeGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            while (CanBeExecuted)
            {
                InteractableGameAction interactableGameAction = new(isUndable: true, Description);
                Create(interactableGameAction);
                await _gameActionsProvider.Create(interactableGameAction);
            }
        }

        private void Create(InteractableGameAction interactableGameAction)
        {
            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                if (!CanChoose()) continue;

                interactableGameAction.Create()
               .SetCard(card)
               .SetInvestigator(ActiveInvestigator)
               .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Discard))
               .SetLogic(Discard);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create(new DiscardGameAction(card));
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
