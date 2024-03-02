namespace MythosAndHorrors.GameRules
{
    public class CardAvatar : Card
    {
        public override CardInfo Info => Owner.InvestigatorCard.Info;
    }
}
