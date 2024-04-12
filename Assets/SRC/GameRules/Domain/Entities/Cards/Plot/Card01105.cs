using System.Collections.Generic;
using System.Linq;
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
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(DiscardAllInvestigators))
                .SetLogic(DiscardAllInvestigators);

            interactableGameAction.Create()
              .SetCard(this)
              .SetInvestigator(_investigatorProvider.Leader)
              .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Damage))
              .SetLogic(Damage);


            await _gameActionsProvider.Create(interactableGameAction);
        }

        IEnumerable<Investigator> InvestigatorsWithCards => _investigatorProvider.AllInvestigatorsInPlay
            .Where(investigator => investigator.HandZone.Cards.Any());

        private async Task DiscardAllInvestigators()
        {
            Investigator investigator = InvestigatorsWithCards.FirstOrDefault();
            while (investigator != null)
            {
                await Discard(investigator);
                investigator = InvestigatorsWithCards.NextElementFor(investigator);
            }
        }

        private async Task Discard(Investigator investigator)
        {
            Card cardToDiscard = investigator.HandZone.Cards.Rand();
            await _gameActionsProvider.Create(new DiscardGameAction(cardToDiscard));
        }

        private async Task Damage()
        {
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(_investigatorProvider.Leader, amountFear: 2));
        }
    }
}
