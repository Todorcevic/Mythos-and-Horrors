using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckMaxHandSizeGameAction : InteractableGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public CheckMaxHandSizeGameAction(Investigator investigator) :
            base(canBackToThisInteractable: true, mustShowInCenter: false, nameof(CheckMaxHandSizeGameAction), investigator)
        { }

        public override void ExecuteSpecificInitialization()
        {
            if (ActiveInvestigator.HandSize <= ActiveInvestigator.MaxHandSize.Value) CreateContinueMainButton();
            else CreateGameActions();
        }

        /*******************************************************************/
        private void CreateGameActions()
        {
            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                if (!CanChoose()) continue;

                Create(card, Discard, PlayActionType.None, ActiveInvestigator);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create(new DiscardGameAction(card));
                    await _gameActionsProvider.Create(new CheckMaxHandSizeGameAction(ActiveInvestigator));
                };

                bool CanChoose()
                {
                    if (card.Tags.Contains(Tag.Weakness)) return false;
                    return true;
                }
            }
        }
    }
}
