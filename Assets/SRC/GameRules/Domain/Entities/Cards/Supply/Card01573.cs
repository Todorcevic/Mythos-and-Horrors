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
            CreateOptativeReaction<InvestigatePlaceGameAction>(Condition, Logic, GameActionTime.After, "OptativeReaction_Card01573");
        }

        /*******************************************************************/
        private async Task Logic(InvestigatePlaceGameAction investigatePlaceGameAction)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01573");

            foreach (Card itemCard in ControlOwner.DiscardZone.Cards.Where(card => card.HasThisTag(Tag.Item)))
            {
                interactableGameAction.CreateEffect(itemCard, new Stat(0, false), DrawItem, PlayActionType.Choose | PlayActionType.Draw, ControlOwner, "CardEffect_Card01573");

                /*******************************************************************/
                async Task DrawItem() => await _gameActionsProvider.Create<DrawGameAction>().SetWith(ControlOwner, itemCard).Execute();
            }

            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
            await interactableGameAction.Execute();
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
    }
}
