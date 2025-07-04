﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01157 : CardColosus, IStalker, IVictoriable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IEnumerable<Investigator> InvestigatorsVictoryAffected => _investigatorsProvider.AllInvestigators;
        int IVictoriable.Victory => 10;
        bool IVictoriable.IsVictoryComplete => Defeated.IsActive;
        public override IEnumerable<Tag> Tags => new[] { Tag.AncientOne, Tag.Elite };
        public State Defeated { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            RemoveStat(Health);
            Health = CreateStat((Info.Health ?? 0) + _investigatorsProvider.AllInvestigators.Count() * 4);
            Defeated = CreateState(false);
            CreateForceReaction<InvestigatorsPhaseGameAction>(ReadyCondition, ReadyLogic, GameActionTime.After);
            CreateForceReaction<DefeatCardGameAction>(DefeatCondition, Defeat, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task ReadyLogic(InvestigatorsPhaseGameAction action)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, false).Execute();
        }

        private bool ReadyCondition(InvestigatorsPhaseGameAction action)
        {
            if (IsInPlay.IsFalse) return false;
            if (!Exausted.IsActive) return false;
            return true;
        }

        private bool DefeatCondition(DefeatCardGameAction defeatCardGameAction)
        {
            if (defeatCardGameAction.Card != this) return false;
            return true;
        }

        private async Task Defeat(DefeatCardGameAction defeatCardGameAction)
        {
            if (HealthLeft < 1) await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Defeated, true).Execute();
        }
    }
}
