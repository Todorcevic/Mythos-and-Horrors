namespace MythosAndHorrors.GameRules
{
    public interface IAbility
    {
        string Description { get; }
        void Disable();
        void Enable();
    }
}
