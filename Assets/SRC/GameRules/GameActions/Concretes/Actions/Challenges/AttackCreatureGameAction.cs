using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class AttackCreatureGameAction : ChallengePhaseGameAction
    {
        public CardCreature CardCreature { get; private set; }
        public Stat AmountDamage { get; private set; }

        /*******************************************************************/
        public AttackCreatureGameAction SetWith(Investigator investigator, CardCreature creature, int amountDamage)
        {
            SetWith(investigator.Strength, creature.Strength.Value, "Attack " + creature.Info.Name, cardToChallenge: creature);
            CardCreature = creature;
            AmountDamage = new Stat(amountDamage, false);
            SuccesEffects.Add(SuccesEffet);
            FailEffects.Add(FailEffet);
            return this;
        }

        /*******************************************************************/
        private async Task SuccesEffet() =>
            await _gameActionsProvider.Create(new HarmToCardGameAction(CardCreature, ActiveInvestigator.InvestigatorCard, amountDamage: AmountDamage.Value));

        private async Task FailEffet()
        {
            if (CardCreature.IsConfronted && CardCreature.ConfrontedInvestigator != ActiveInvestigator)
                await _gameActionsProvider.Create(new HarmToCardGameAction(CardCreature.ConfrontedInvestigator.InvestigatorCard, ActiveInvestigator.InvestigatorCard, amountDamage: AmountDamage.Value));
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

            await _gameActionsProvider.Create(new CreatureAttackGameAction(CardCreature, ActiveInvestigator));
        }
    }
}
