namespace MythsAndHorrors.GameRules
{
    public interface ICardActivator
    {
        void ActivateThisCards(params Card[] gameActions);
    }
}
