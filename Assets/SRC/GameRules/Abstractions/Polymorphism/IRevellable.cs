namespace MythosAndHorrors.GameRules
{
    public interface IRevellable
    {
        State Revealed { get; }
        History RevealHistory { get; }
    }
}
