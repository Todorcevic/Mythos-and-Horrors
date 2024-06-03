using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01596 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Madness };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Card");
            foreach (Card card in investigator.DiscardableCardsInHand)
            {
                interactableGameAction.Create(card, Discard, PlayActionType.Choose, investigator: investigator);

                async Task Discard()
                {
                    await _gameActionsProvider.Create(new SafeForeach<Card>(() => investigator.HandZone.Cards.Where(card => card.CanBeDiscarded).Except(new[] { card }), Logic));

                    async Task Logic(Card card) => await _gameActionsProvider.Create(new DiscardGameAction(card));
                }
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
