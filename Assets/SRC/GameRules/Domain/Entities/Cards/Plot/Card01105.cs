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

        IEnumerable<Investigator> InvestigatorsWithCards() => _investigatorProvider.AllInvestigatorsInPlay
        .Where(investigator => investigator.HandZone.Cards.Any());

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select One Effect");
            interactableGameAction.Create(this, DiscardAllInvestigators, PlayActionType.None, investigator: _investigatorProvider.Leader);
            interactableGameAction.Create(this, Damage, PlayActionType.None, investigator: _investigatorProvider.Leader, cardAffected: _investigatorProvider.Leader.InvestigatorCard);
            await _gameActionsProvider.Create(interactableGameAction);
        }

        private async Task DiscardAllInvestigators()
        {
            await _gameActionsProvider.Create(new SafeForeach<Investigator>(InvestigatorsWithCards, Discard));

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
