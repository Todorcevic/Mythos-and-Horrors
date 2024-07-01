using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class HealthGameAction : GameAction
    {

        public int AmountDamageToRecovery { get; }
        public int AmountFearToRecovery { get; }
        public Card CardAffected { get; }

        /*******************************************************************/
        public HealthGameAction(Card card, int amountDamageToRecovery = 0, int amountFearToRecovery = 0)
        {
            CardAffected = card;
            AmountDamageToRecovery = amountDamageToRecovery;
            AmountFearToRecovery = amountFearToRecovery;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (CardAffected is IDamageable damageable)
            {
                int amountToRetrieve = AmountDamageToRecovery > damageable.DamageRecived.Value ? damageable.DamageRecived.Value : AmountDamageToRecovery;
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(damageable.DamageRecived, amountToRetrieve).Start();
            }

            if (CardAffected is IFearable fearable)
            {
                int amountToRetrieve = AmountFearToRecovery > fearable.FearRecived.Value ? fearable.FearRecived.Value : AmountFearToRecovery;
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(fearable.FearRecived, amountToRetrieve).Start();
            }
        }
    }
}

