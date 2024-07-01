using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01520 : CardWeapon
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Melee };

        /*******************************************************************/
        protected override async Task ExtraAttackEnemyLogic(AttackCreatureGameAction attackCreatureGameAction)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.StatModifier, 1).Start();
            if (ControlOwner.AllTypeCreaturesConfronted.UniqueOrDefault() == attackCreatureGameAction.CardCreature)
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.AmountDamage, 1).Start();
        }
    }
}
