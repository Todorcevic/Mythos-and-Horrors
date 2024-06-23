using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01529 : CardWeapon, IBulletable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Weapon, Tag.Firearm };
        public Stat AmountBullets { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AmountBullets = CreateStat(2);
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
            await _gameActionsProvider.Create(new IncrementStatGameAction(attackCreatureGameAction.StatModifier, 3));

            attackCreatureGameAction.SuccesEffects.Clear();
            attackCreatureGameAction.FailEffects.Clear();
            attackCreatureGameAction.SuccesEffects.Add(SuccessEffect);
            attackCreatureGameAction.FailEffects.Add(FailEffect);

            async Task FailEffect()
            {
                int amountDamage = Math.Clamp(attackCreatureGameAction.ResultChallenge.TotalDifferenceValue, -5, -1) * -1;
                if (attackCreatureGameAction.CardCreature.IsConfronted && attackCreatureGameAction.CardCreature.ConfrontedInvestigator != ControlOwner)
                    await _gameActionsProvider.Create(new HarmToCardGameAction(attackCreatureGameAction.CardCreature.ConfrontedInvestigator.InvestigatorCard, ControlOwner.InvestigatorCard, amountDamage: amountDamage));
            }

            async Task SuccessEffect()
            {

                int amountDamage = Math.Clamp(attackCreatureGameAction.ResultChallenge.TotalDifferenceValue, 1, 5);
                await _gameActionsProvider.Create(new HarmToCardGameAction(attackCreatureGameAction.CardCreature, ControlOwner.InvestigatorCard, amountDamage: amountDamage));
            }
        }


    }
}
