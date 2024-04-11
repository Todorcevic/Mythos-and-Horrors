using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class HarmToCardGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Card Card { get; }
        public int AmountDamage { get; private set; }
        public int AmountFear { get; private set; }
        public int TotalDamageApply { get; private set; }
        public int TotalFearApply { get; private set; }

        /*******************************************************************/
        public HarmToCardGameAction(Card card, int amountDamage = 0, int amountFear = 0)
        {
            Card = card;
            AmountDamage = amountDamage;
            AmountFear = amountFear;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> statsWithValues = new();

            if (Card is IDamageable damageable)
            {
                TotalDamageApply = AmountDamage < damageable.Health.Value ? AmountDamage : damageable.Health.Value;
                statsWithValues.Add(damageable.Health, TotalDamageApply);
            }

            if (Card is IFearable fearable)
            {
                TotalFearApply = AmountFear < fearable.Sanity.Value ? AmountFear : fearable.Sanity.Value;
                statsWithValues.Add(fearable.Sanity, TotalFearApply);
            }
            await _gameActionsProvider.Create(new DecrementStatGameAction(statsWithValues));
        }
    }
}
