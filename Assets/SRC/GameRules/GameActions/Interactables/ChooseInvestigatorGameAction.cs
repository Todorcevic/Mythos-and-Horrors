using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : InteractableGameAction
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public ChooseInvestigatorGameAction SetWith()
        {
            SetWith(canBackToThisInteractable: true, mustShowInCenter: true, "Choose Investigator");
            return this;
        }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
        {
            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsCanStartTurn)
            {
                CreateEffect(investigator.AvatarCard, new Stat(0, false), PlayInvestigator, PlayActionType.Choose, investigator);

                /*******************************************************************/
                async Task PlayInvestigator() => await _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            }
        }
    }
}
