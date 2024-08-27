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
            await _gameActionsProvider.Create<ChallengePhaseGameAction>().SetWith(investigator.Power, 4, "Challenge_Card01167", this, failEffect: DiscardOrFear, localizableArgs: Info.Name).Execute();

            /*******************************************************************/
            async Task DiscardOrFear()
            {
                IEnumerable<CardSupply> cardSuppliesForDiscard = investigator.AidZone.Cards.OfType<CardSupply>().Where(supply => supply.CanBeDiscarted.IsTrue);
                if (cardSuppliesForDiscard.Any())
                {
                    InteractableGameAction interactableGameAcrtion = _gameActionsProvider.Create<InteractableGameAction>()
                        .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01167");

                    foreach (CardSupply cardSupply in cardSuppliesForDiscard)
                    {
                        interactableGameAcrtion.CreateCardEffect(cardSupply, new Stat(0, false), Discard, PlayActionType.Choose, playedBy: investigator, "CardEffect_Card01167");

                        /*******************************************************************/
                        async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(cardSupply).Execute();
                    }

                    await interactableGameAcrtion.Execute();
                }

                else await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, fromCard: this, amountDamage: 2).Execute();
            }
        }
    }
}
