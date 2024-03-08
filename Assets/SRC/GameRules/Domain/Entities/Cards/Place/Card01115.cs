namespace MythosAndHorrors.GameRules
{
    public class Card01115 : CardPlace
    {
        protected override bool CanMove()
        {
            if (!Revealed.IsActive) return false;
            return base.CanMove();
        }
    }
}
