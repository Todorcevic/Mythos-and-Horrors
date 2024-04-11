using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01105 : CardPlot
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        public Effect DiscardEffect { get; private set; }
        public Effect DamageEffect { get; private set; }

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            InteractableGameAction interactableGameAction = new(isUndable: false);

            interactableGameAction.Create()
                .SetCard(this)
                .SetInvestigator(_investigatorProvider.Leader)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(DiscardEffect))
                .SetLogic(async () => await _gameActionsProvider.Create(new DiscardGameAction(this)));

            interactableGameAction.Create()
              .SetCard(this)
              .SetInvestigator(_investigatorProvider.Leader)
              .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(DamageEffect))
              .SetLogic(async () => await _gameActionsProvider.Create(new DiscardGameAction(this)));


            await _gameActionsProvider.Create(interactableGameAction);
        }

        private void Discard()
        {

        }

        private void Damage()
        {

        }
    }
}
