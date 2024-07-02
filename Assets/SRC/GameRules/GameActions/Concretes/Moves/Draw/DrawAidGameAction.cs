namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : DrawGameAction
    {
        public DrawAidGameAction SetWith(Investigator investigator)
        {
            SetWith(investigator, investigator.CardAidToDraw);
            return this;
        }
    }
}
