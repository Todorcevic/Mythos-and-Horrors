namespace MythosAndHorrors.GameRules
{
    public interface ICommitable
    {
        State Commited { get; }
        int GetChallengeValue(ChallengeType challengeType);
    }
}
