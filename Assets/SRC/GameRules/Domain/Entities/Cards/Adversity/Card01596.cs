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
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Select Card");
            foreach (Card card in investigator.DiscardableCardsInHand)
            {
                interactableGameAction.CreateEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, playedBy: investigator);

                async Task Discard()
                {
                    await _gameActionsProvider.Create<SafeForeach<Card>>().SetWith(Collection, Logic).Execute();

                    /*******************************************************************/
                    IEnumerable<Card> Collection() => investigator.HandZone.Cards.Where(card => card.CanBeDiscarted.IsActive).Except(new[] { card });
                    async Task Logic(Card card) => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                }
            }

            await interactableGameAction.Execute();
        }
    }
}
