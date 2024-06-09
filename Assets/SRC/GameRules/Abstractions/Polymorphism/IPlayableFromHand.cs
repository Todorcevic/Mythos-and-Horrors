namespace MythosAndHorrors.GameRules
{
    public interface IPlayableFromHand
    {
        PlayActionType PlayFromHandActionType { get; }
        Stat ResourceCost { get; }
        Stat PlayFromHandTurnsCost { get; }
        GameCommand<PlayFromHandGameAction> PlayFromHandCommand { get; }
        GameCondition<GameAction> PlayFromHandCondition { get; }
    }
}
