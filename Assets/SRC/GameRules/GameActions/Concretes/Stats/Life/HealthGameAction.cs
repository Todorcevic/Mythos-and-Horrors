using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class HealthGameAction : GameAction
    {
        public int AmountDamageToRecovery { get; private set; }
        public int AmountFearToRecovery { get; private set; }
        public Card CardAffected { get; private set; }

        /*******************************************************************/
        public HealthGameAction SetWith(Card card, int amountDamageToRecovery = 0, int amountFearToRecovery = 0)
        {
            CardAffected = card;
            AmountDamageToRecovery = amountDamageToRecovery;
            AmountFearToRecovery = amountFearToRecovery;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (CardAffected is IDamageable damageable)
            {
                int amountToRetrieve = AmountDamageToRecovery > damageable.DamageRecived.Value ? damageable.DamageRecived.Value : AmountDamageToRecovery;
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(damageable.DamageRecived, amountToRetrieve).Execute();
            }

            if (CardAffected is IFearable fearable)
            {
                int amountToRetrieve = AmountFearToRecovery > fearable.FearRecived.Value ? fearable.FearRecived.Value : AmountFearToRecovery;
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(fearable.FearRecived, amountToRetrieve).Execute();
            }
        }
    }
}

