namespace MythsAndHorrors.GameRules
{
    public class CardAvatar : Card
    {
        public override CardInfo Info => Owner.InvestigatorCard.Info;
    }
}
