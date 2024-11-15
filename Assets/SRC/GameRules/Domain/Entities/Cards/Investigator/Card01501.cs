﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01501 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public State AbilityUsed { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Agency, Tag.Detective };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            AbilityUsed = CreateState(false);
            CreateOptativeReaction<DefeatCardGameAction>(DiscoverKeyCondition, DiscoverKeyLogic, GameActionTime.After, new Localization("OptativeReaction_Card01501"));
            CreateForceReaction<RoundGameAction>(RestartAbilityCondition, RestartAbilityLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        protected override async Task StarEffect() => await Task.CompletedTask;

        protected override int StarValue() => Owner.CurrentPlace.Keys.Value;

        /*******************************************************************/
        private async Task DiscoverKeyLogic(DefeatCardGameAction defeatCardGameAction)
        {
            await _gameActionsProvider.Create<GainKeyGameAction>().SetWith(Owner, Owner.CurrentPlace.Keys, 1).Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, true).Execute();
        }

        private bool DiscoverKeyCondition(DefeatCardGameAction defeatCardGameAction)
        {
            if (defeatCardGameAction.ByThisInvestigator != Owner) return false;
            if (defeatCardGameAction.Card is not CardCreature) return false;
            if (IsInPlay.IsFalse) return false;
            if (Owner.CurrentPlace.Keys.Value < 1) return false;
            if (AbilityUsed.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RestartAbilityLogic(RoundGameAction roundGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, false).Execute();
        }

        private bool RestartAbilityCondition(RoundGameAction roundGameAction)
        {
            if (!AbilityUsed.IsActive) return false;
            return true;
        }
    }
}
