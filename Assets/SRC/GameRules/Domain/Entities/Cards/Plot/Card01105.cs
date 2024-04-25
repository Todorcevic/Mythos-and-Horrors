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
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select One Effect");

            interactableGameAction.Create()
               .SetCard(this)
               .SetInvestigator(_investigatorProvider.Leader)
               .SetLogic(DiscardAllInvestigators);

            interactableGameAction.Create()
               .SetCard(this)
               .SetInvestigator(_investigatorProvider.Leader)
               .SetCardAffected(_investigatorProvider.Leader.InvestigatorCard)
               .SetLogic(Damage);

            await _gameActionsProvider.Create(interactableGameAction);
        }

        private async Task DiscardAllInvestigators()
        {
            await new SafeForeach<Investigator>(Discard, InvestigatorsWithCards).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsWithCards() => _investigatorProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.HandZone.Cards.Any());

            async Task Discard(Investigator investigator)
            {
                Card cardToDiscard = investigator.HandZone.Cards.Rand();
                await _gameActionsProvider.Create(new DiscardGameAction(cardToDiscard));
            }
        }

        private async Task Damage()
        {
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(_investigatorProvider.Leader, this, amountFear: 2));
        }
    }
}
