using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CheckMaxHandSizeGameAction : InteractableGameAction, IPersonalInteractable
    {
        private const string CODE = "CheckMaxHandSize";

        public Investigator ActiveInvestigator { get; private set; }

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string code)
         => throw new NotImplementedException();

        public CheckMaxHandSizeGameAction SetWith(Investigator investigator)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, code: CODE);
            ActiveInvestigator = investigator;
            ExecuteSpecificInitialization();
            return this;
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            if (ActiveInvestigator.HandSize <= ActiveInvestigator.MaxHandSize.Value) CreateContinueMainButton();
            else CreateGameActions();
        }

        /*******************************************************************/
        private void CreateGameActions()
        {
            foreach (Card card in ActiveInvestigator.DiscardableCardsInHand)
            {
                CreateEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, ActiveInvestigator);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                    await _gameActionsProvider.Create<CheckMaxHandSizeGameAction>().SetWith(ActiveInvestigator).Execute();
                };
            }
        }
    }
}
