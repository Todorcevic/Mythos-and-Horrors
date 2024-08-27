using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class AttackCreatureGameAction : ChallengePhaseGameAction
    {
        public CardCreature CardCreature { get; private set; }
        public Stat AmountDamage { get; private set; }
        public override bool CanBeExecuted => ActiveInvestigator.IsInPlay.IsTrue && CardCreature.IsInPlay.IsTrue;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new ChallengePhaseGameAction SetWith(Stat stat, int difficultValue, string localizableCode, Card cardToChallenge, Func<Task> succesEffect = null, Func<Task> failEffect = null, params string[] localizableArgs)
            => throw new NotImplementedException();

        public AttackCreatureGameAction SetWith(Investigator investigator, CardCreature creature, int amountDamage)
        {
            base.SetWith(investigator.Strength, creature.Strength.Value, "Challenge_AttackCreature", cardToChallenge: creature, localizableArgs: creature.Info.Name);
            CardCreature = creature;
            AmountDamage = new Stat(amountDamage, false);
            SuccesEffects.Add(SuccesEffet);
            FailEffects.Add(FailEffet);
            return this;
        }

        /*******************************************************************/
        private async Task SuccesEffet() =>
            await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(CardCreature, ActiveInvestigator.InvestigatorCard, amountDamage: AmountDamage.Value).Execute();

        private async Task FailEffet()
        {
            if (CardCreature.IsConfronted && CardCreature.ConfrontedInvestigator != ActiveInvestigator)
                await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(CardCreature.ConfrontedInvestigator.InvestigatorCard, ActiveInvestigator.InvestigatorCard, amountDamage: AmountDamage.Value).Execute();
        }

        protected override async Task ExecuteThisPhaseLogic()
        {
            await base.ExecuteThisPhaseLogic();
            await CheckCounterAttack();
        }

        /*******************************************************************/
        private async Task CheckCounterAttack()
        {
            if (CardCreature is not ICounterAttackable) return;
            if (CardCreature.Exausted.IsActive) return;
            if (IsSucceed) return;

            await _gameActionsProvider.Create<CreatureAttackGameAction>().SetWith(CardCreature, ActiveInvestigator).Execute();
        }
    }
}
