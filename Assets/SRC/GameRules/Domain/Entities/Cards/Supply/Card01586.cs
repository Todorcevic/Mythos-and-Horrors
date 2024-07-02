using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01586 : CardWeapon
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        public State ThrowingState { get; private set; }

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Melee };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ThrowingState = CreateState(false);
            CreateActivation(1, ThrowAttackLogic, AttackCondition, PlayActionType.Attack);
        }

        /*******************************************************************/
        private async Task ThrowAttackLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ThrowingState, true).Start();
            await ChooseEnemyLogic(investigator);
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ThrowingState, false).Start();
        }

        /*******************************************************************/
        protected override async Task ExtraAttackEnemyLogic(AttackCreatureGameAction attackCreatureGameAction)
        {
            if (ThrowingState.IsActive)
            {
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Start();
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.StatModifier, 2).Start();
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.AmountDamage, 1).Start();
            }
            else
            {
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(attackCreatureGameAction.StatModifier, 1).Start();
            }
        }

    }
}
