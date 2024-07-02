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

        bool IsFreeActivation => PlayFromHandTurnsCost.Value < 1;
        bool WithOppotunityAttack => !IsFreeActivation && (PlayFromHandActionType & PlayActionType.WithoutOpportunityAttack) == PlayActionType.None;
        bool IsJustPlayFromHand => PlayFromHandActionType == PlayActionType.PlayFromHand;
    }
}
