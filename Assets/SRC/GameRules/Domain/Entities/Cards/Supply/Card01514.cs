﻿using System;
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
            CreateForceReaction<DiscardGameAction>(MoveToDeckCondition, MoveToDeckConditionLogic, GameActionTime.Initial);
            CreateBuff(CardsToBuff, ActivationBuff, DeactivationBuff, new Localization("Buff_Card01514"));
        }

        /*******************************************************************/
        private async Task ActivationBuff(IEnumerable<Card> enumerable)
        {
            if (enumerable.FirstOrDefault() is not CardCondition cardCondition) return;
            if (cardCondition is CardConditionReaction cardConditionReaction)
            {
                Func<GameAction, bool> originalCondition = cardConditionReaction.PlayFromHandCondition.ConditionLogic;
                cardConditionReaction.PlayFromHandCondition.UpdateWith(ConditionToPlayFromHand);
                await Task.CompletedTask;

                /*******************************************************************/
                bool ConditionToPlayFromHand(GameAction gameAction)
                {
                    Zone discardZone = cardCondition.CurrentZone;
                    Zone handZone = cardCondition.ControlOwner.HandZone;
                    discardZone.RemoveCard(cardCondition);
                    handZone.AddCard(cardCondition);
                    bool result = originalCondition.Invoke(gameAction);
                    handZone.RemoveCard(cardCondition);
                    discardZone.AddCard(cardCondition);
                    return result;
                }
            }
            else if (cardCondition is CardConditionPlayFromHand cardConditionPlayFromHans)
            {
                Func<Investigator, bool> originalCondition = cardConditionPlayFromHans.PlayFromHandCondition.ConditionLogic;
                cardConditionPlayFromHans.PlayFromHandCondition.UpdateWith(ConditionToPlayFromHand);
                await Task.CompletedTask;

                /*******************************************************************/
                bool ConditionToPlayFromHand(Investigator investigator)
                {
                    Zone discardZone = cardCondition.CurrentZone;
                    Zone handZone = cardCondition.ControlOwner.HandZone;
                    discardZone.RemoveCard(cardCondition);
                    handZone.AddCard(cardCondition);
                    bool result = originalCondition.Invoke(investigator);
                    handZone.RemoveCard(cardCondition);
                    discardZone.AddCard(cardCondition);
                    return result;
                }
            }
        }

        private async Task DeactivationBuff(IEnumerable<Card> enumerable)
        {
            if (enumerable.FirstOrDefault() is CardConditionReaction cardCondition)
            {
                cardCondition.PlayFromHandCondition.Reset();
                await Task.CompletedTask;
            }
            else if (enumerable.FirstOrDefault() is CardConditionPlayFromHand cardConditionPlayFromHand)
            {
                cardConditionPlayFromHand.PlayFromHandCondition.Reset();
                await Task.CompletedTask;
            }
        }

        private IEnumerable<Card> CardsToBuff() =>
            IsInPlay.IsTrue ? new List<Card>() { ControlOwner.DiscardZone.Cards.LastOrDefault() } : Enumerable.Empty<Card>();

        /*******************************************************************/
        private async Task MoveToDeckConditionLogic(DiscardGameAction discardGameAction)
        {
            discardGameAction.Cancel();
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(discardGameAction.Cards, ControlOwner.DeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ChangeCardPositionGameAction>().SetWith(discardGameAction.Cards.First(), 0).Execute();
        }

        private bool MoveToDeckCondition(DiscardGameAction discardGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (discardGameAction.Parent is not PlayEffectGameAction playEffectGameAction) return false;
            if (playEffectGameAction.Effect.Investigator != ControlOwner) return false;
            if (!playEffectGameAction.Effect.IsThatActionType(PlayActionType.PlayFromHand)) return false;
            if (playEffectGameAction.Effect is not CardEffect cardEffect) return false;
            if (cardEffect.CardOwner is not CardCondition) return false;
            return true;
        }
    }
}
