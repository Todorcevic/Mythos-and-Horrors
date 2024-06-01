namespace MythosAndHorrors.GameRules
{
    public interface IPlayableFromHand
    {
        Stat ResourceCost { get; }
        Stat PlayFromHandTurnsCost { get; }
        GameCommand<PlayFromHandGameAction> PlayFromHandCommand { get; }
        GameCondition<GameAction> PlayFromHandCondition { get; }
    }
}
