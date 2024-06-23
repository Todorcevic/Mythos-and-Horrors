using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01544 : CardWeapon
    {
        private AttackCreatureGameAction _attackCreatureGameAction;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Melee, Tag.Illicit };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandTurnsCost = CreateStat(0);
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
        protected override async Task ExtraAttackEnemyLogic(AttackCreatureGameAction attackCreatureGameAction)
        {
            _attackCreatureGameAction = attackCreatureGameAction;
            await Task.CompletedTask;
        }
    }
}
