using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class AttackGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }
        public CardCreature CardCreature { get; }
        public int AmountDamage { get; }

        /*******************************************************************/
        public AttackGameAction(Investigator investigator, CardCreature creature, int amountDamage)
        {
            Investigator = investigator;
            CardCreature = creature;
            AmountDamage = amountDamage;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ChallengePhaseGameAction challenge = await _gameActionsProvider.Create(new ChallengePhaseGameAction(
                Investigator.Strength,
                CardCreature.Strength.Value,
                "Attack " + CardCreature.Info.Name,
                succesEffect: SuccesEffet,
                cardToChallenge: CardCreature));

            await CheckCounterAttack(challenge);

            /*******************************************************************/
            async Task SuccesEffet() => await _gameActionsProvider.Create(new HarmToCardGameAction(CardCreature, Investigator.InvestigatorCard, amountDamage: AmountDamage));
        }

        private async Task CheckCounterAttack(ChallengePhaseGameAction challenge)
        {
            if (CardCreature is not ICounterAttackable) return;
            if (CardCreature.Exausted.IsActive) return;
            if (challenge.IsSuccessful ?? true) return;

            await _gameActionsProvider.Create(new CreatureAttackGameAction(CardCreature, Investigator));
        }
    }
}
