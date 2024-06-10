using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01167 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Hazard };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new ChallengePhaseGameAction(investigator.Power, 4, "Crypt Challenge", this, failEffect: DiscardOrFear));

            /*******************************************************************/
            async Task DiscardOrFear()
            {
                IEnumerable<CardSupply> cardSuppliesForDiscard = investigator.AidZone.Cards.OfType<CardSupply>().Where(supply => supply.CanBeDiscarded);
                if (cardSuppliesForDiscard.Any())
                {
                    InteractableGameAction interactableGameAcrtion = new(canBackToThisInteractable: false, mustShowInCenter: true, "Discard", investigator);

                    foreach (CardSupply cardSupply in cardSuppliesForDiscard)
                    {
                        interactableGameAcrtion.CreateEffect(cardSupply, Discard, PlayActionType.Choose, playedBy: investigator);

                        /*******************************************************************/
                        async Task Discard() => await _gameActionsProvider.Create(new DiscardGameAction(cardSupply));
                    }

                    await _gameActionsProvider.Create(interactableGameAcrtion);
                }

                else await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, fromCard: this, amountDamage: 2));
            }
        }
    }
}
