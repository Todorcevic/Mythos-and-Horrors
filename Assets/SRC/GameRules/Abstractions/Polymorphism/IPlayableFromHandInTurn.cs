using System;

namespace MythosAndHorrors.GameRules
{
    public interface IPlayableFromHandInTurn
    {
        PlayActionType PlayFromHandActionType { get; }
        Func<Card> CardAffected { get; }
        Stat PlayFromHandTurnsCost { get; }
        Stat ResourceCost { get; }
        GameCommand<GameAction> PlayFromHandCommand { get; }
        GameConditionWith<Investigator> PlayFromHandCondition { get; }
    }
}
