namespace MythosAndHorrors.GameRules.News
{
    public class Free : Triggered
    {
        public Card Card { get; }

        public PlayActionType PlayAction { get; }

        public Free(Card card, PlayActionType playAction)
        {
            Card = card;
            PlayAction = playAction;
        }
    }
}
