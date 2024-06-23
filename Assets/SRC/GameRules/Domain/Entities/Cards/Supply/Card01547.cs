using ModestTree;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01547 : CardWeapon
    {
        private AttackCreatureGameAction _attackCreatureGameAction;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Firearm, Tag.Illicit };
        public Stat AmountBullets { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AmountBullets = CreateStat(3);
            CreateForceReaction<ResolveChallengeGameAction>(Condition, Logic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task Logic(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            if (resolveChallengeGameAction.ChallengePhaseGameAction is not AttackCreatureGameAction attackCreatureGameAction) return;
            await _gameActionsProvider.Create(new IncrementStatGameAction(attackCreatureGameAction.AmountDamage, 1));
        }

        private bool Condition(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            if (resolveChallengeGameAction.ChallengePhaseGameAction != _attackCreatureGameAction) return false;
            if (_attackCreatureGameAction.ResultChallenge.TotalDifferenceValue < 2) return false;
            return true;
        }

        /*******************************************************************/
        protected override bool AttackCondition(Investigator investigator)
        {
            if (AmountBullets.Value <= 0) return false;
            return base.AttackCondition(investigator);
        }

        protected override async Task ExtraAttackEnemyLogic(AttackCreatureGameAction attackCreatureGameAction)
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(AmountBullets, 1));
            await _gameActionsProvider.Create(new IncrementStatGameAction(attackCreatureGameAction.StatModifier, 2));
            _attackCreatureGameAction = attackCreatureGameAction;
        }
    }
}
