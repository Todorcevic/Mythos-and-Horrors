namespace MythosAndHorrors.GameRules
{
    public class FromHand : Triggered
    {
        public Card Card { get; }

        public PlayActionType PlayAction { get; }

        public FromHand(Card card, PlayActionType playAction)
        {
            Card = card;
            PlayAction = playAction;
        }
    }
}
