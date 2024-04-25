using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckMaxHandSizeGameAction : InteractableGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        //public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(CheckMaxHandSizeGameAction);
        //public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(CheckMaxHandSizeGameAction);
        //public override Phase MainPhase => Phase.Restore;
        public Investigator ActiveInvestigator { get; }

        //public override bool CanBeExecuted => ActiveInvestigator.HandSize > ActiveInvestigator.MaxHandSize.Value;

        /*******************************************************************/
        public CheckMaxHandSizeGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            CanBackToThisInteractable = true;
            MustShowInCenter = false;
            Description = nameof(CheckMaxHandSizeGameAction);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CreateUndoButton().SetLogic(Undo);

            if (ActiveInvestigator.HandSize <= ActiveInvestigator.MaxHandSize.Value)
            {
                CreateMainButton().SetLogic(Continue);
            }
            else CreateGameActions();

            await base.ExecuteThisLogic();

            if (EffectSelected == MainButtonEffect || EffectSelected == UndoEffect) return;
            await _gameActionsProvider.Create(new CheckMaxHandSizeGameAction(ActiveInvestigator));

            /*******************************************************************/

            async Task Undo()
            {
                InteractableGameAction lastInteractable = await _gameActionsProvider.UndoLastInteractable();
                lastInteractable.ClearEffects();
                await _gameActionsProvider.Create(lastInteractable);
            }

            async Task Continue() => await Task.CompletedTask;

        }

        private void CreateGameActions()
        {
            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                if (!CanChoose()) continue;

                Create()
               .SetCard(card)
               .SetInvestigator(ActiveInvestigator)
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
