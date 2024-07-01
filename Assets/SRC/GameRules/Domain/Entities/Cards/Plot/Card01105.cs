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

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Select One Effect");
            interactableGameAction.CreateEffect(this, new Stat(0, false), DiscardAllInvestigators, PlayActionType.Choose, playedBy: _investigatorProvider.Leader);
            interactableGameAction.CreateEffect(this, new Stat(0, false), Damage, PlayActionType.Choose, playedBy: _investigatorProvider.Leader, cardAffected: _investigatorProvider.Leader.InvestigatorCard);
            await interactableGameAction.Start();
        }

        private async Task DiscardAllInvestigators()
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsWithCards, Discard).Start();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsWithCards() =>
                _investigatorProvider.AllInvestigatorsInPlay.Where(investigator => investigator.HandZone.Cards.Any());

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
