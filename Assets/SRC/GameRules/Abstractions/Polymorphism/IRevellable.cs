namespace MythsAndHorrors.GameRules
{
    public interface IRevellable
    {
        State IsRevealed { get; }
        History RevealHistory { get; }
    }
}
