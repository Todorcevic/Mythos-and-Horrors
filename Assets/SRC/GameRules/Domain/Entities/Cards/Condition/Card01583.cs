﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01583 : CardConditionReaction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };
        protected override GameActionTime FastReactionAtStart => GameActionTime.After;

        protected override Localization Localization => new("OptativeReaction_Card01583");

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not EludeGameAction eludeGameAction) return false;
            if (eludeGameAction.Creature.CurrentPlace != ControlOwner.CurrentPlace) return false;
            if (eludeGameAction.Creature.HasThisTag(Tag.Elite)) return true;
            if (eludeGameAction.Creature.HasThisTag(Tag.Weakness)) return true;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not EludeGameAction eludeGameAction) return;
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(eludeGameAction.Creature, _chaptersProvider.CurrentScene.DangerDeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ResetCardGameAction>().SetWith(eludeGameAction.Creature).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(_chaptersProvider.CurrentScene.DangerDeckZone).Execute();
        }
    }
}
