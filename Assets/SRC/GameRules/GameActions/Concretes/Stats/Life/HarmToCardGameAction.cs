using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class HarmToCardGameAction : GameAction
    {
        public Card Card { get; private set; }
        public Card ByThisCard { get; private set; }
        public int AmountDamage { get; private set; }
        public int AmountFear { get; private set; }
        public int TotalDamageApply { get; private set; }
        public int TotalFearApply { get; private set; }

        /*******************************************************************/
        public HarmToCardGameAction SetWith(Card card, Card byThisCard, int amountDamage = 0, int amountFear = 0)
        {
            Card = card;
            ByThisCard = byThisCard;
            AmountDamage = amountDamage;
            AmountFear = amountFear;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> statsWithValues = new();

            if (Card is IDamageable damageable)
            {
                TotalDamageApply = AmountDamage < damageable.HealthLeft ? AmountDamage : damageable.HealthLeft;
                statsWithValues.Add(damageable.DamageRecived, TotalDamageApply);
            }

            if (Card is IFearable fearable)
            {
                TotalFearApply = AmountFear < fearable.SanityLeft ? AmountFear : fearable.SanityLeft;
                statsWithValues.Add(fearable.FearRecived, TotalFearApply);
            }
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(statsWithValues).Start();

            if (Card is IDamageable damageableAfter)
            {
                if (damageableAfter.HealthLeft <= 0)
                {
                    await _gameActionsProvider.Create<DefeatCardGameAction>().SetWith(Card, ByThisCard).Start();
                }
            }
            if (Card is IFearable fearableAfter)
            {
                if (fearableAfter.SanityLeft <= 0)
                {
                    await _gameActionsProvider.Create<DefeatCardGameAction>().SetWith(Card, ByThisCard).Start();
                }
            }
        }
    }
}
