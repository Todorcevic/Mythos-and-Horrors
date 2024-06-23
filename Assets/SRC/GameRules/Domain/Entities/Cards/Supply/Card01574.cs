using ModestTree;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01574 : CardWeapon
    {
        private AttackCreatureGameAction _attackCreatureGameAction;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Melee };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<ResolveChallengeGameAction>(Condition, Logic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task Logic(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        private bool Condition(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            if (resolveChallengeGameAction.ChallengePhaseGameAction != _attackCreatureGameAction) return false;
            if (resolveChallengeGameAction.ChallengePhaseGameAction.ResultChallenge.TokensRevealed
                .All(token => token.TokenType != ChallengeTokenType.Fail && token.TokenType != ChallengeTokenType.Creature)) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task ExtraAttackEnemyLogic(AttackCreatureGameAction attackCreatureGameAction)
        {
            await _gameActionsProvider.Create(new IncrementStatGameAction(attackCreatureGameAction.StatModifier, 2));
            await _gameActionsProvider.Create(new IncrementStatGameAction(attackCreatureGameAction.AmountDamage, 1));
            _attackCreatureGameAction = attackCreatureGameAction;
        }
    }
}
