using System.Diagnostics.CodeAnalysis;
using System;

namespace MythosAndHorrors.GameRules
{
    public class ResetConditionalGameAction : UpdateConditionalGameAction
    {
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new UpdateConditionalGameAction SetWith(Conditional conditional, bool? value)
            => throw new NotImplementedException();

        public UpdateConditionalGameAction SetWith(Conditional conditional)
        {
            base.SetWith(conditional, null);
            return this;
        }
    }
}
