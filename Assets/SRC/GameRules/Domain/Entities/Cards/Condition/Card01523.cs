﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01523 : CardConditionReaction
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        protected override GameActionTime FastReactionAtStart => GameActionTime.Before;

        protected override Localization Localization => new("OptativeReaction_Card01523");

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not CreatureAttackGameAction creatureAttackGameAction) return false;
            if (creatureAttackGameAction.Creature.IsInPlay.IsFalse) return false;
            if (!ControlOwner.CurrentPlace.InvestigatorsInThisPlace.Contains(creatureAttackGameAction.Investigator)) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not CreatureAttackGameAction creatureAttackGameAction) return;
            creatureAttackGameAction.Cancel();
            await Task.CompletedTask;
        }
    }
}
