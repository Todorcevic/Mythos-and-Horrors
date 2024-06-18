namespace MythosAndHorrors.GameRules
{
    public interface ITriggered : IAbility
    {
        public Card Card { get; }
        public PlayActionType PlayAction { get; }
    }
}
