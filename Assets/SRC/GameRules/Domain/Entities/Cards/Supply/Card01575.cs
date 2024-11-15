﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01575 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Charm };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateOptativeReaction<ChallengePhaseGameAction>(Condition, Logic, GameActionTime.After, new Localization("OptativeReaction_Card01575"));
        }

        /*******************************************************************/
        private async Task Logic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
            await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(ControlOwner).Execute();
        }

        private bool Condition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (challengePhaseGameAction.ActiveInvestigator != ControlOwner) return false;
            if (challengePhaseGameAction.IsSucceed) return false;
            return true;
        }

        /*******************************************************************/
    }
}
