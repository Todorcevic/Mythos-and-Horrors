using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : InteractableGameAction, IInitializable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public ChooseInvestigatorGameAction() : base(canBackToThisInteractable: true, mustShowInCenter: true, "Choose Investigator") { }

        /*******************************************************************/
        public void ExecuteSpecificInitialization()
        {
            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsCanStartTurn)
            {
                Create().SetCard(investigator.AvatarCard)
                    .SetInvestigator(investigator)
                    .SetLogic(PlayInvestigator);

                /*******************************************************************/
                async Task PlayInvestigator()
                {
                    await _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
                };
            }
        }
    }
}
