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
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Relic };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<DiscardGameAction>(MoveToDeckCondition, MoveToDeckConditionLogic, true);
            CreateBuff(CardsToBuff, ActivationBuff, DeactivationBuff);
        }

        /*******************************************************************/
        private async Task ActivationBuff(IEnumerable<Card> enumerable)
        {
            if (enumerable.FirstOrDefault() is not CardCondition cardCondition) return;
            cardCondition.PlayFromHandReaction.NewCondition(ConditionToPlayFromHand);
            await Task.CompletedTask;

            /*******************************************************************/
            bool ConditionToPlayFromHand(GameAction gameAction)
            {
                Zone discardZone = cardCondition.CurrentZone;
                Zone handZone = cardCondition.ControlOwner.HandZone;
                discardZone.RemoveCard(cardCondition);
                handZone.AddCard(cardCondition);
                bool result = cardCondition.PlayFromHandCondition(gameAction);
                handZone.RemoveCard(cardCondition);
                discardZone.AddCard(cardCondition);
                return result;
            }
        }

        private async Task DeactivationBuff(IEnumerable<Card> enumerable)
        {
            if (enumerable.FirstOrDefault() is not CardCondition cardCondition) return;
            cardCondition.PlayFromHandReaction.NewCondition(cardCondition.PlayFromHandCondition);
            await Task.CompletedTask;
        }

        private IEnumerable<Card> CardsToBuff() =>
            IsInPlay ? new List<Card>() { ControlOwner.DiscardZone.Cards.LastOrDefault() } : Enumerable.Empty<Card>();

        /*******************************************************************/
        private async Task MoveToDeckConditionLogic(DiscardGameAction discardGameAction)
        {
            discardGameAction.Cancel();
            await _gameActionsProvider.Create(new MoveCardsGameAction(discardGameAction.Card, ControlOwner.DeckZone, isFaceDown: true));
            await _gameActionsProvider.Create(new ShuffleGameAction(ControlOwner.DeckZone)); //Original card is move to last position
        }

        private bool MoveToDeckCondition(DiscardGameAction discardGameAction)
        {
            if (!IsInPlay) return false;
            if (discardGameAction.Parent is not PlayFromHandGameAction playFromHandGameAction) return false;
            if (playFromHandGameAction.Investigator != ControlOwner) return false;
            if (playFromHandGameAction.Card is not CardCondition) return false;
            return true;
        }
    }
}
