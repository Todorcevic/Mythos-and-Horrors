using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResetCardGameAction : GameAction
    {
        public Card Card { get; private set; }

        /*******************************************************************/
        public ResetCardGameAction SetWith(Card card)
        {
            Card = card;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Card is IResetable resetable) await resetable.Reset();

            Dictionary<Stat, int> allStats = new();
            if (Card is IChargeable chargeable)
                allStats.Add(chargeable.Charge.Amount, chargeable.Charge.Amount.InitialValue);
            if (Card is IDamageable damageable)
                allStats.Add(damageable.DamageRecived, damageable.DamageRecived.InitialValue);
            if (Card is IFearable fearable)
                allStats.Add(fearable.FearRecived, fearable.FearRecived.InitialValue);
            if (Card is IEldritchable eldritchable)
                allStats.Add(eldritchable.Eldritch, eldritchable.Eldritch.InitialValue);

            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Card.Exausted, false).Execute();
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(allStats).Execute();
        }
    }
}
