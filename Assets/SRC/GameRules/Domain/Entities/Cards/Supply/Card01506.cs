using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01506 : CardWeapon, IBulletable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Firearm };
        public Stat AmountBullets { get; private set; }


        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AmountBullets = CreateStat(4);
        }

        /*******************************************************************/
        protected override bool AttackCondition(Investigator investigator)
        {
            if (AmountBullets.Value <= 0) return false;
            return base.AttackCondition(investigator);
        }

        protected override async Task AttackEnemy(CardCreature creature)
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(AmountBullets, 1));
            int strengtIncrement = ControlOwner.CurrentPlace.Hints.Value > 0 ? 3 : 1;
            await _gameActionsProvider.Create(new AttackGameAction(ControlOwner, creature, amountDamage: 2, strengModifier: strengtIncrement));
        }
    }
}
