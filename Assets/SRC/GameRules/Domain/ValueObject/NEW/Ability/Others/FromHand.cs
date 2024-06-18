namespace MythosAndHorrors.GameRules
{
    public class FromHand : ITriggered
    {
        public Card Card { get; }

        public PlayActionType PlayAction { get; }

        public string Description => throw new System.NotImplementedException();

        public FromHand(Card card, PlayActionType playAction)
        {
            Card = card;
            PlayAction = playAction;
        }

        public void Disable()
        {
            throw new System.NotImplementedException();
        }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }
    }
}
