using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01573 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Talent };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateOptativeReaction<InvestigatePlaceGameAction>(Condition, Logic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task Logic(InvestigatePlaceGameAction investigatePlaceGameAction)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Item");
            interactableGameAction.CreateCancelMainButton(_gameActionsProvider);

            foreach (Card itemCard in ControlOwner.DiscardZone.Cards.Where(card => card.HasThisTag(Tag.Item)))
            {
                interactableGameAction.CreateEffect(itemCard, new Stat(0, false), DrawItem, PlayActionType.Choose | PlayActionType.Draw, ControlOwner);

                /*******************************************************************/
                async Task DrawItem() => await _gameActionsProvider.Create(new DrawGameAction(ControlOwner, itemCard));
            }

            await _gameActionsProvider.Create(new UpdateStatesGameAction(Exausted, true));
            await _gameActionsProvider.Create(interactableGameAction);
        }

        private bool Condition(InvestigatePlaceGameAction investigatePlaceGameAction)
        {
            if (!IsInPlay) return false;
            if (investigatePlaceGameAction.ActiveInvestigator != ControlOwner) return false;
            if (!investigatePlaceGameAction.IsSucceed) return false;
            if (investigatePlaceGameAction.ResultChallenge.TotalDifferenceValue < 2) return false;
            if (!ControlOwner.DiscardZone.Cards.Where(card => card.HasThisTag(Tag.Item)).Any()) return false;
            return true;
        }

        /*******************************************************************/
    }
}
