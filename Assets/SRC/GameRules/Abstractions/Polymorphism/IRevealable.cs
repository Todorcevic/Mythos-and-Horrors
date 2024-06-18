namespace MythosAndHorrors.GameRules
{

    public interface IRevealable
    {
        State Revealed { get; }
        GameCommand<RevealGameAction> RevealCommand { get; }
    }
}
