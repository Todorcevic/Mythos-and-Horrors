namespace MythosAndHorrors.GameRules
{
    public class DrawDangerGameAction : DrawGameAction
    {
        public DrawDangerGameAction SetWith(Investigator investigator)
        {
            SetWith(investigator, investigator.CardDangerToDraw);
            return this;
        }
    }
}
