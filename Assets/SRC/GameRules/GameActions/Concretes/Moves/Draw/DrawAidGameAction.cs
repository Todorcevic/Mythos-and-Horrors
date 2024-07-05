using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : DrawGameAction
    {
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new DrawGameAction SetWith(Investigator investigator, Card cardDrawed)
           => throw new NotImplementedException();

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new DrawGameAction SetWith(Investigator investigator, IEnumerable<Card> cardsDrawed)
            => throw new NotImplementedException();

        public DrawAidGameAction SetWith(Investigator investigator) => SetWith(investigator, 1);

        public DrawAidGameAction SetWith(Investigator investigator, int amount)
        {
            base.SetWith(investigator, investigator.DeckZone.Cards.TakeLast(amount));
            return this;
        }
    }
}
