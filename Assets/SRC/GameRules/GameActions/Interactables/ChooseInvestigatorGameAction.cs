using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : InteractableGameAction
    {
        private const string CODE = "ChooseInvestigator";
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string code, params string[] args)
         => throw new NotImplementedException();

        public ChooseInvestigatorGameAction SetWith()
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: true, code: CODE);
            ExecuteSpecificInitialization();
            return this;
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
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
