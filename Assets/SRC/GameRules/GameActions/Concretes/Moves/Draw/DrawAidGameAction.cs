using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : DrawGameAction
    {
        public int Amount { get; private set; }

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new DrawGameAction SetWith(Investigator investigator, Card cardDrawed)
           => throw new NotImplementedException();

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new DrawGameAction SetWith(Investigator investigator, IEnumerable<Card> cardsDrawed)
            => throw new NotImplementedException();

        public DrawAidGameAction SetWith(Investigator investigator) => SetWith(investigator, 1);

        public DrawAidGameAction SetWith(Investigator investigator, int amount)
        {
            Amount = amount;
            base.SetWith(investigator, investigator.DeckZone.Cards.TakeLast(amount));
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
        }
    }
}
