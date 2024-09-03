namespace MythosAndHorrors.GameRules
{
    public interface IAbility : IViewEffectDescription
    {
        void Disable();
        void Enable();
    }
}
