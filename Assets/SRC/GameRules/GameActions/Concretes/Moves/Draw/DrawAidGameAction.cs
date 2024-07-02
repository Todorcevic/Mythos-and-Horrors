using System.Diagnostics.CodeAnalysis;
using System;

namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : DrawGameAction
    {
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new DrawGameAction SetWith(Investigator investigator, Card cardDrawed)
           => throw new NotImplementedException();

        public DrawAidGameAction SetWith(Investigator investigator)
        {
            base.SetWith(investigator, investigator.CardAidToDraw);
            return this;
        }
    }
}
