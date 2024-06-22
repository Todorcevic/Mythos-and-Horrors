using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class AttackCreatureGameAction : ChallengePhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardCreature CardCreature { get; }
        public Stat AmountDamage { get; private set; }

        /*******************************************************************/
        public AttackCreatureGameAction(Investigator investigator, CardCreature creature, int amountDamage)
            : base(investigator.Strength, creature.Strength.Value, "Attack " + creature.Info.Name, cardToChallenge: creature)
        {
            CardCreature = creature;
            AmountDamage = new Stat(amountDamage, false);
            SuccesEffects.Add(SuccesEffet);
            FailEffects.Add(FailEffet);
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
