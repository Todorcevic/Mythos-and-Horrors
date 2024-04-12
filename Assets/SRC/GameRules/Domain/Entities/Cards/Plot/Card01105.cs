using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01105 : CardPlot
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            InteractableGameAction interactableGameAction = new(isUndable: false, "Select One Effect");

            interactableGameAction.Create()
                .SetCard(this)
                .SetInvestigator(_investigatorProvider.Leader)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Discard))
                .SetLogic(Discard);

            interactableGameAction.Create()
              .SetCard(this)
              .SetInvestigator(_investigatorProvider.Leader)
              .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Damage))
              .SetLogic(Damage);


            await _gameActionsProvider.Create(interactableGameAction);
        }

        private async Task Discard()
        {

        }

        private async Task Damage()
        {

        }
    }
}
