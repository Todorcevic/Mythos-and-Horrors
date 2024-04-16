using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        //public IEnumerable<Investigator> InvestigatorsToSelect { get; }
        public Investigator InvestigatorSelected { get; private set; }

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChooseInvestigatorGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChooseInvestigatorGameAction);
        public override Phase MainPhase => Phase.Investigator;

        public override bool CanBeExecuted => _investigatorsProvider.GetInvestigatorsCanStartTurn.Count() > 0;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, Description);
            interactableGameAction.CreateUndoButton().SetLogic(UndoEffect);

            async Task UndoEffect()
            {
                await _gameActionsProvider.UndoLastInteractable();
                await _gameActionsProvider.Create(new PlayInvestigatorGameAction(_gameActionsProvider.LastPhasePlayed.ActiveInvestigator));
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
                    await _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
                };
            }

            await _gameActionsProvider.Create(interactableGameAction);
            await _gameActionsProvider.Create(new ChooseInvestigatorGameAction());
        }
    }
}
