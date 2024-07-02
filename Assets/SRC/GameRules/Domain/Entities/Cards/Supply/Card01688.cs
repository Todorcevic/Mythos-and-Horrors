using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01688 : CardWeapon, IChargeable
    {
        private AttackCreatureGameAction _attackCreatureGameAction;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Firearm, Tag.Illicit };
        public Charge Charge { get; private set; }
        public State Used { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(3, ChargeType.Bullet);
            Used = CreateState(false);
            CreateForceReaction<ResolveChallengeGameAction>(ExtraDamageCondition, ExtraDamageLogic, GameActionTime.Before);
            CreateForceReaction<PlayInvestigatorGameAction>(ResetCondition, ResetLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task ResetLogic(PlayInvestigatorGameAction action)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Used, false).Execute();
        }

        private bool ResetCondition(PlayInvestigatorGameAction action)
        {
            if (!Used.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        private async Task ExtraDamageLogic(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            if (resolveChallengeGameAction.ChallengePhaseGameAction is not AttackCreatureGameAction attackCreatureGameAction) return;
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.AmountDamage, 1).Execute();
        }

        private bool ExtraDamageCondition(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            if (resolveChallengeGameAction.ChallengePhaseGameAction != _attackCreatureGameAction) return false;
            if (_attackCreatureGameAction.ResultChallenge.TotalDifferenceValue < 2) return false;
            return true;
        }

        /*******************************************************************/
        protected override bool AttackCondition(Investigator investigator)
        {
            if (Charge.Amount.Value <= 0) return false;
            return base.AttackCondition(investigator);
        }

        protected override async Task ExtraAttackEnemyLogic(AttackCreatureGameAction attackCreatureGameAction)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.StatModifier, 2).Execute();
            _attackCreatureGameAction = attackCreatureGameAction;
            attackCreatureGameAction.SuccesEffects.Add(ExtraTurn);

            async Task ExtraTurn()
            {
                if (Used.IsActive) return;
                if (!attackCreatureGameAction.IsSucceed) return;
                if (attackCreatureGameAction.ResultChallenge.TotalDifferenceValue < 3) return;

                await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Used, true).Execute();
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.ActiveInvestigator.CurrentTurns, 1).Execute();
            }
        }


    }
}
