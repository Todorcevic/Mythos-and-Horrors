using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public ChooseInvestigatorGameAction() : base(canBackToThisInteractable: true, mustShowInCenter: true, "Choose Investigator") { }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsCanStartTurn)
            {
                Create(investigator.AvatarCard, PlayInvestigator, PlayActionType.Choose, investigator);

                /*******************************************************************/
                async Task PlayInvestigator()
                {
                    await _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
                };
            }
        }
    }
}
