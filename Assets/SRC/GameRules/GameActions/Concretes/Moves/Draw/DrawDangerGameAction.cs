namespace MythosAndHorrors.GameRules
{
    public class DrawDangerGameAction : DrawGameAction
    {
        public DrawDangerGameAction(Investigator investigator) : base(investigator, investigator.CardDangerToDraw) { }
    }
}
