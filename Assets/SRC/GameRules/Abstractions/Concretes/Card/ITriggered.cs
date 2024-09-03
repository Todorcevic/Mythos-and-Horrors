namespace MythosAndHorrors.GameRules
{
    public interface ITriggered : IAbility
    {
        Card Card { get; }
        PlayActionType PlayAction { get; }
    }
}
