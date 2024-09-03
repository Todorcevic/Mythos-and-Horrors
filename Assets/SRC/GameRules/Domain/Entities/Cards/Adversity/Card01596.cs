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
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01596"));
            foreach (Card card in investigator.DiscardableCardsInHand)
            {
                interactableGameAction.CreateCardEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, playedBy: investigator, new Localization("CardEffect_Card01596"));

                async Task Discard()
                {
                    IEnumerable<Card> toDiscard = investigator.HandZone.Cards.Where(card => card.CanBeDiscarted.IsTrue).Except(new[] { card });
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(toDiscard).Execute();
                }
            }

            await interactableGameAction.Execute();
        }
    }
}
