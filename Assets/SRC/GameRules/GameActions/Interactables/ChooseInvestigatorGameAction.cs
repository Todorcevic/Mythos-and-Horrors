using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IEnumerable<Investigator> InvestigatorsToSelect { get; }
        public Investigator InvestigatorSelected { get; private set; }

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChooseInvestigatorGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChooseInvestigatorGameAction);
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        public ChooseInvestigatorGameAction(IEnumerable<Investigator> investigatorsToSelect)
        {
            InvestigatorsToSelect = investigatorsToSelect;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            InteractableGameAction interactableGameAction = new(isUndable: true, Description);
            foreach (Investigator investigator in InvestigatorsToSelect)
            {
                interactableGameAction.Create()
                    .SetCard(investigator.AvatarCard)
                    .SetInvestigator(investigator)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(ChooseInvestigator))
                    .SetLogic(ChooseInvestigator);

                /*******************************************************************/
                async Task ChooseInvestigator()
                {
                    InvestigatorSelected = investigator;
                    await Task.CompletedTask;
                };
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
