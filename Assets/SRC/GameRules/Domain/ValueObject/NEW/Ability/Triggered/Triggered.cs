namespace MythosAndHorrors.GameRules.News
{

    public class Triggered : Ability
    {
        public Triggered(PlayActionType playAction)
        {
            PlayAction = playAction;
        }

        public PlayActionType PlayAction { get; }
    }
}
