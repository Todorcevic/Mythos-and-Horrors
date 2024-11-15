﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01578 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand | PlayActionType.Elude;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<SafeForeach<CardCreature>>().SetWith(AllCreatures, EvedaAction).Execute();

            /*******************************************************************/
            IEnumerable<CardCreature> AllCreatures() => investigator.CreaturesInSamePlace;
            async Task EvedaAction(CardCreature creature) => await _gameActionsProvider.Create<EludeGameAction>().SetWith(creature, investigator).Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!ControlOwner.CreaturesInSamePlace.Any()) return false;
            return true;
        }
    }
}
