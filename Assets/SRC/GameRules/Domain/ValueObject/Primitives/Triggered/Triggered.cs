namespace MythosAndHorrors.GameRules
{
    public interface Triggered
    {
        public Card Card { get; }
        public PlayActionType PlayAction { get; }
    }
}
