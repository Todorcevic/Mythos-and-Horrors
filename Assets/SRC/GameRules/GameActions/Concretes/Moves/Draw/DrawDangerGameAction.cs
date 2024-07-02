using System;
using System.Diagnostics.CodeAnalysis;

namespace MythosAndHorrors.GameRules
{
    public class DrawDangerGameAction : DrawGameAction
    {
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new DrawGameAction SetWith(Investigator investigator, Card cardDrawed)
            => throw new NotImplementedException();

        public DrawDangerGameAction SetWith(Investigator investigator)
        {
            base.SetWith(investigator, investigator.CardDangerToDraw);
            return this;
        }
    }
}
