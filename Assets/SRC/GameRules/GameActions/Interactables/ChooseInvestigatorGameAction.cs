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
        protected override async Task ExecuteThisLogic()
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

            await base.ExecuteThisLogic();
        }
    }
}
