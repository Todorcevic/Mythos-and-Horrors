using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01560 : CardWeapon, IChargeable
    {
        private AttackCreatureGameAction _attackCreatureGameAction;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spell };
        public Charge Charge { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(4, ChargeType.MagicCharge);
            CreateForceReaction<ResolveChallengeGameAction>(ResolveCondition, ResolveLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task ResolveLogic(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(ControlOwner, this, amountFear: 1));
        }

        private bool ResolveCondition(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            if (resolveChallengeGameAction.ChallengePhaseGameAction != _attackCreatureGameAction) return false;
            if (resolveChallengeGameAction.ChallengePhaseGameAction.ResultChallenge.TokensRevealed
                .All(token => ((int)token.TokenType) < 10 || ((int)token.TokenType) > 50)) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task ExtraAttackEnemyLogic(AttackCreatureGameAction attackCreatureGameAction)
        {
            attackCreatureGameAction.ChangeStat(ControlOwner.Power);
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Start();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.AmountDamage, 1).Start();
            _attackCreatureGameAction = attackCreatureGameAction;
        }
    }
}
