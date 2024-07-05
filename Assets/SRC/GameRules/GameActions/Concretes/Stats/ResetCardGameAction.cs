using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResetCardGameAction : GameAction
    {
        public IEnumerable<Card> Cards { get; private set; }

        /*******************************************************************/
        public ResetCardGameAction SetWith(Card card) => SetWith(new Card[] { card });

        public ResetCardGameAction SetWith(IEnumerable<Card> cards)
        {
            Cards = cards;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Stat, int> allStats = new();

            foreach (Card card in Cards)
            {
                if (card is IResetable resetable) await resetable.Reset();
                if (card is IChargeable chargeable)
                    allStats.Add(chargeable.Charge.Amount, chargeable.Charge.Amount.InitialValue);
                if (card is IDamageable damageable)
                    allStats.Add(damageable.DamageRecived, damageable.DamageRecived.InitialValue);
                if (card is IFearable fearable)
                    allStats.Add(fearable.FearRecived, fearable.FearRecived.InitialValue);
                if (card is IEldritchable eldritchable)
                    allStats.Add(eldritchable.Eldritch, eldritchable.Eldritch.InitialValue);
            }

            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Cards.Select(card => card.Exausted), false).Execute();
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(allStats).Execute();
        }
    }
}
