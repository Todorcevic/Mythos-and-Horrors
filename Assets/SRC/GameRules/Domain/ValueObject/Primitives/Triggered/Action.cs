namespace MythosAndHorrors.GameRules
{
    public class Action : Triggered
    {
        public Card Card { get; }

        public PlayActionType PlayAction { get; }

        public Action(Card card, PlayActionType playAction)
        {
            Card = card;
            PlayAction = playAction;
        }
    }
}
