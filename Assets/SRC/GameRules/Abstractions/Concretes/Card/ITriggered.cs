namespace MythosAndHorrors.GameRules
{
    public interface ITriggered : IAbility
    {
        string LocalizableCode { get; }
        string[] LocalizableArgs { get; }
        Card Card { get; }
        PlayActionType PlayAction { get; }
    }
}
