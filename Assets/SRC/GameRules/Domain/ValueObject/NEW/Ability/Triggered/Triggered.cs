namespace MythosAndHorrors.GameRules.News
{
    public interface Triggered
    {
        public Card Card { get; }
        public PlayActionType PlayAction { get; }
    }
}
