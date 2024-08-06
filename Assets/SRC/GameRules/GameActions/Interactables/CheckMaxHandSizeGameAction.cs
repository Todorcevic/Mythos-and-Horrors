using System;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckMaxHandSizeGameAction : InteractableGameAction, IPersonalInteractable
    {
        [Inject] private readonly TextsProvider _textsProvider;

        public Investigator ActiveInvestigator { get; private set; }

        /*******************************************************************/
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string description)
         => throw new NotImplementedException();

        public CheckMaxHandSizeGameAction SetWith(Investigator investigator)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, nameof(CheckMaxHandSizeGameAction));
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
