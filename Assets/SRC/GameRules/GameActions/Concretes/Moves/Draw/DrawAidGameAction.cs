namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : DrawGameAction
    {
        public DrawAidGameAction(Investigator investigator) : base(investigator, investigator.CardAidToDraw) { }
    }
}
