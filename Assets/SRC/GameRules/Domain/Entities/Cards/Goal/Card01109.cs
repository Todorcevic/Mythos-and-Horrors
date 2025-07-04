﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01109 : CardGoal
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        private CardPlace Parlor => _cardsProvider.GetCard<Card01115>();
        private CardPlace Hallway => _cardsProvider.GetCard<Card01112>();
        private Card Lita => _cardsProvider.GetCard<Card01117>();
        private CardCreature GhoulPriest => _cardsProvider.GetCard<Card01116>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            CreateOptativeReaction<RoundGameAction>(PayKeysCondition, PayKeysLogic, GameActionTime.Before, new Localization("OptativeReaction_Card01109"));
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create<RevealGameAction>().SetWith(Parlor).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Lita, Parlor.OwnZone).Execute();
            await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(GhoulPriest, Hallway).Execute();
        }

        /*******************************************************************/
        private bool PayKeysCondition(RoundGameAction roundGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (Revealed.IsActive) return false;
            if (_investigatorsProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == Hallway).Sum(investigator => investigator.Keys.Value) < Keys.Value) return false;
            return true;
        }

        private async Task PayKeysLogic(RoundGameAction roundGameAction)
        {
            await _gameActionsProvider.Create<SafeWhile>().SetWith(Condition, PayKey).Execute();

            /*******************************************************************/
            bool Condition() => _investigatorsProvider.AllInvestigatorsInPlay.Any(investigator => investigator.CanPayKeys.IsTrue) && IsInPlay.IsTrue && !Revealed.IsActive && Keys.Value > 0;
            async Task PayKey()
            {
                IEnumerable<Investigator> specificInvestigators = _investigatorsProvider.AllInvestigatorsInPlay
                        .Where(investigator => investigator.CurrentPlace == Hallway && investigator.CanPayKeys.IsTrue);
                await _gameActionsProvider.Create<PayKeysToGoalGameAction>().SetWith(this, specificInvestigators).Execute();
            }
        }

        protected override bool PayKeysConditionToActivate(Investigator investigator) => false;
    }
}