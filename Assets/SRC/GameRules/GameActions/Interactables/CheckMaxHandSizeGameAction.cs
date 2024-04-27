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
            base(canBackToThisInteractable: true, mustShowInCenter: false, nameof(CheckMaxHandSizeGameAction))
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (ActiveInvestigator.HandSize <= ActiveInvestigator.MaxHandSize.Value) CreateMainButton().SetLogic(Continue);
            else CreateGameActions();

            await base.ExecuteThisLogic();

            /*******************************************************************/

            async Task Continue() => await Task.CompletedTask;
        }

        private void CreateGameActions()
        {
            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                if (!CanChoose()) continue;

                Create()
               .SetCard(card)
               .SetInvestigator(ActiveInvestigator)
               .SetLogic(Discard);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create(new DiscardGameAction(card));
                    await _gameActionsProvider.Create(new CheckMaxHandSizeGameAction(ActiveInvestigator));
                };

                bool CanChoose()
                {
                    if (card.Tags.Contains(Tag.Flaw)) return false;
                    return true;
                }
            }
        }
    }
}
