namespace MythsAndHorrors.GameRules
{
    public class Card01115 : CardPlace
    {
        public override bool CanMoveWithThis(Investigator investigator)
        {
            if (!IsRevealed.Value) return false;
            return base.CanMoveWithThis(investigator);
        }
    }
}
