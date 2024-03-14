namespace MythosAndHorrors.GameRules
{
    public interface IPlayableFromHand
    {
        Stat ResourceCost { get; }
        Stat TurnsCost { get; }
    }
}
