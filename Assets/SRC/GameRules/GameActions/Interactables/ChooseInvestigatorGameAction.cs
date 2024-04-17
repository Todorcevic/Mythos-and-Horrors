using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChooseInvestigatorGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChooseInvestigatorGameAction);
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, Description);
            interactableGameAction.CreateUndoButton().SetLogic(UndoEffect);

            async Task UndoEffect()
            {
                InteractableGameAction lastInteractable = await _gameActionsProvider.UndoLastInteractable();
                if (lastInteractable.Parent is OneInvestigatorTurnGameAction oneInvestigator)
                    await _gameActionsProvider.Create(oneInvestigator.Parent);
            }

            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsCanStartTurn)
            {
                interactableGameAction.Create()
                    .SetCard(investigator.AvatarCard)
                    .SetInvestigator(investigator)
                    .SetLogic(PlayInvestigator);

                /*******************************************************************/
                async Task PlayInvestigator()
                {
                    await _gameActionsProvider.Create(new PlayInvestigatorLoopGameAction(investigator));
                };
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
