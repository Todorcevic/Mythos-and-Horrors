using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01514 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Relic };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<DiscardGameAction>(PlayConditionCondition, PlayConditionLogic, true);
            CreateReaction<OneInvestigatorTurnGameAction>(PlayTopDiscardCondition, PlayTopDiscardLogic, true);
        }

        private async Task PlayTopDiscardLogic(OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
        {
            oneInvestigatorTurnGameAction.Create().SetCard(this)
                    .SetInvestigator(oneInvestigatorTurnGameAction.ActiveInvestigator)
                    .SetLogic(PlayFromHand);
            await Task.CompletedTask;

            /*******************************************************************/
            async Task PlayFromHand() =>
                await _gameActionsProvider.Create(new PlayFromHandGameAction(this, oneInvestigatorTurnGameAction.ActiveInvestigator));
        }

        private bool PlayTopDiscardCondition(OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
        {
            if (!IsInPlay) return false;
            if (oneInvestigatorTurnGameAction.ActiveInvestigator != ControlOwner) return false;
            if (ControlOwner.DiscardZone.Cards.LastOrDefault() is not CardCondition cardCondition) return false;
            if (cardCondition is not IPlayableFromHand playableFromHand) return false;
            return DefaultCondition(playableFromHand);

            bool DefaultCondition(IPlayableFromHand playableFromHand)
            {
                if (playableFromHand.ResourceCost.Value > ControlOwner.Resources.Value) return false;
                if (playableFromHand.PlayFromHandTurnsCost.Value > ControlOwner.CurrentTurns.Value) return false;
                if (!playableFromHand.SpecificConditionToPlayFromHand()) return false;
                return true;
            }
        }

        /*******************************************************************/
        private async Task PlayConditionLogic(DiscardGameAction discardGameAction)
        {
            discardGameAction.Cancel();
            await _gameActionsProvider.Create(new MoveCardsGameAction(discardGameAction.Card, ControlOwner.DeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new ShuffleGameAction(ControlOwner.DeckZone)); //Original card is move to last position
        }

        private bool PlayConditionCondition(DiscardGameAction discardGameAction)
        {
            if (!IsInPlay) return false;
            if (discardGameAction.Parent is not PlayFromHandGameAction playFromHandGameAction) return false;
            if (playFromHandGameAction.Investigator != ControlOwner) return false;
            if (playFromHandGameAction.Card is not CardCondition) return false;
            return true;
        }

        /*******************************************************************/
    }
}
