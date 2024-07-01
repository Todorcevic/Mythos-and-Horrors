using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01506 : CardWeapon, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Firearm };
        public Charge Charge { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(4, ChargeType.Bullet);
        }

        /*******************************************************************/
        protected override bool AttackCondition(Investigator investigator)
        {
            if (Charge.Amount.Value <= 0) return false;
            return base.AttackCondition(investigator);
        }

        protected override async Task ExtraAttackEnemyLogic(AttackCreatureGameAction attackCreatureGameAction)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Start();
            int strengtIncrement = ControlOwner.CurrentPlace.Hints.Value > 0 ? 3 : 1;
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.StatModifier, strengtIncrement).Start();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.AmountDamage, 1).Start();
        }
    }
}
