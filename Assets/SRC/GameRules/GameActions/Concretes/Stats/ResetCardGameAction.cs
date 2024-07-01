using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResetCardGameAction : GameAction
    {

        private Card Card { get; }

        /*******************************************************************/
        public ResetCardGameAction(Card card)
        {
            Card = card;
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

            await _gameActionsProvider.Create(new UpdateStatesGameAction(Card.Exausted, false));
            await _gameActionsProvider.Create(new UpdateStatGameAction(allStats));
        }
    }
}
