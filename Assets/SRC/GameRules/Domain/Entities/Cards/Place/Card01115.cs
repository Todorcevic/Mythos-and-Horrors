using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class Card01115 : CardPlace
    {
        //[Inject]
        //[SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        //private void Init()
        //{
        //    CanMove.UpdateCondition(() => Revealed.IsActive && CanMove.Result);
        //}

        public override bool CanMoveWithThis(Investigator investigator)
        {
            if (!Revealed.IsActive) return false;
            return base.CanMoveWithThis(investigator);
        }
    }
}
