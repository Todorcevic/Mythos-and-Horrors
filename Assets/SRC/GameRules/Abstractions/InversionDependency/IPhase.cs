namespace MythsAndHorrors.GameRules
{
    public interface IPhase
    {
        Phase MainPhase { get; }
        string Name { get; }
        string Description { get; }
    }
}
