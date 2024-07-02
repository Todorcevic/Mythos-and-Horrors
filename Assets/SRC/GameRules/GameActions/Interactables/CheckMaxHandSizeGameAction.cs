using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckMaxHandSizeGameAction : InteractableGameAction, IPersonalInteractable
    {
        [Inject] private readonly TextsProvider _textsProvider;

        public Investigator ActiveInvestigator { get; private set; }

        /*******************************************************************/
        public CheckMaxHandSizeGameAction SetWith(Investigator investigator)
        {
            SetWith(canBackToThisInteractable: true, mustShowInCenter: false, nameof(CheckMaxHandSizeGameAction));
            ActiveInvestigator = investigator;
            return this;
        }

        /*******************************************************************/
        public override void ExecuteSpecificInitialization()
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
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Start();
                    await _gameActionsProvider.Create<CheckMaxHandSizeGameAction>().SetWith(ActiveInvestigator).Start();
                };
            }
        }
    }
}
