namespace MythsAndHorrors.GameRules
{
    public interface IRevellable
    {
        History RevealHistory { get; }
        void Reveal();
    }
}
