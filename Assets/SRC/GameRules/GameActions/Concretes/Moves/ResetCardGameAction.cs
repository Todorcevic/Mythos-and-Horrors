using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResetCardGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

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
            if (Card is IBulletable bulletable)
                allStats.Add(bulletable.AmountBullets, bulletable.AmountBullets.InitialValue);
            if (Card is IChargeable chargeable)
                allStats.Add(chargeable.AmountCharges, chargeable.AmountCharges.InitialValue);
            if (Card is ISupplietable supplietable)
                allStats.Add(supplietable.AmountSupplies, supplietable.AmountSupplies.InitialValue);
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
