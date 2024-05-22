namespace MythosAndHorrors.GameRules
{

    public interface ITarget
    {
        bool IsUniqueTarget => false;
        Investigator TargetInvestigator { get; }
    }
}
