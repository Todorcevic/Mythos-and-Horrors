namespace MythosAndHorrors.GameRules
{

    public interface IFearable
    {
        Stat Sanity { get; }
        Stat FearRecived { get; }
        int SanityLeft => Sanity.Value - FearRecived.Value;
        bool CanBeRestoreSanity => FearRecived.Value > 0;
    }
}
