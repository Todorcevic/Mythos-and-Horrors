namespace MythosAndHorrors.GameRules
{
    public class Action : ITriggered
    {
        public Card Card { get; }

        public PlayActionType PlayAction { get; }

        public string Description => throw new System.NotImplementedException();

        public Action(Card card, PlayActionType playAction)
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
