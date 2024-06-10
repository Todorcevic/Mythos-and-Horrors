namespace MythosAndHorrors.GameRules
{
    public interface IPlayableFromHand
    {
        PlayActionType PlayFromHandActionType { get; }
        Stat ResourceCost { get; }
        Stat PlayFromHandTurnsCost { get; }
        GameCommand<Investigator> PlayFromHandCommand { get; }
        GameCondition<GameAction> PlayFromHandCondition { get; }

        bool IsFreeActivation => PlayFromHandTurnsCost.Value < 1;
        bool WithOppotunityAttack => !IsFreeActivation && (PlayFromHandActionType & PlayActionType.WithoutOpportunityAttack) == PlayActionType.None;
        bool IsJustPlayFromHand => PlayFromHandActionType == PlayActionType.PlayFromHand;
    }
}
