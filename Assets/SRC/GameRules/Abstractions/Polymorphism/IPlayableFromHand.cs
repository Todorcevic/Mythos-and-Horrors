using System;

namespace MythosAndHorrors.GameRules
{
    public interface IPlayableFromHand
    {
        PlayActionType PlayFromHandActionType { get; }
        Func<Card> CardAffected { get; }
        Stat ResourceCost { get; }
        Stat PlayFromHandTurnsCost { get; }
        GameCommand<GameAction> PlayFromHandCommand { get; }
        GameConditionWith<Investigator> PlayFromHandCondition { get; }
    }
}
