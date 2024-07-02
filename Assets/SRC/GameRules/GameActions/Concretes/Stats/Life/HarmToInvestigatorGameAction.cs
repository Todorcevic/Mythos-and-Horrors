using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class HarmToInvestigatorGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Card FromCard { get; private set; }
        public int AmountDamage { get; private set; }
        public int AmountFear { get; private set; }
        public bool IsDirect { get; private set; }
        public override bool CanBeExecuted => AmountDamage > 0 || AmountFear > 0;

        /*******************************************************************/
        public HarmToInvestigatorGameAction SetWith(Investigator investigator, Card fromCard, int amountDamage = 0, int amountFear = 0, bool isDirect = false)
        {
            Investigator = investigator;
            FromCard = fromCard;
            AmountDamage = amountDamage;
            AmountFear = amountFear;
            IsDirect = isDirect;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            List<Card> allSelectables = new();
            if (AmountDamage > 0)
                allSelectables.AddRange(Investigator.AidZone.Cards.OfType<IDamageable>().Cast<Card>().Except(allSelectables));
            if (AmountFear > 0)
                allSelectables.AddRange(Investigator.AidZone.Cards.OfType<IFearable>().Cast<Card>().Except(allSelectables));

            if (IsDirect || !allSelectables.Any())
                await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(Investigator.InvestigatorCard, FromCard, AmountDamage, AmountFear).Execute();
            else await _gameActionsProvider.Create<ShareDamageAndFearGameAction>()
                    .SetWith(Investigator, FromCard, AmountDamage, AmountFear).Execute();
        }

        public void AddAmountDamage(int amountDamage) => AmountDamage = Math.Max(0, AmountDamage + amountDamage);

        public void AddAmountFear(int amountFear) => AmountFear = Math.Max(0, AmountFear + amountFear);

    }
}
