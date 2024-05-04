namespace MythosAndHorrors.GameRules
{

    public interface ITarget
    {
        bool IsOnlyOneTarget => false;
        Investigator TargetInvestigator { get; }
    }
}
