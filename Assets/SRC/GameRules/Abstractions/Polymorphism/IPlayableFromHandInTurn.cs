using System;

namespace MythosAndHorrors.GameRules
{
    public interface IPlayableFromHandInTurn
    {
        public bool IsFast { get; }
        PlayActionType PlayFromHandActionType { get; }
        Func<Card> CardAffected { get; }
        Stat ResourceCost { get; }
        GameCommand<GameAction> PlayFromHandCommand { get; }
        GameConditionWith<Investigator> PlayFromHandCondition { get; }
    }
}
