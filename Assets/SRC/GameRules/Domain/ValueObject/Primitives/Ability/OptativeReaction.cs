namespace MythosAndHorrors.GameRules
{
    public class OptativeReaction<T> : Reaction<T>, ITriggered where T : GameAction
    {
        public string LocalizableCode { get; }
        public string[] LocalizableArgs { get; }
        public Card Card { get; }
        public PlayActionType PlayAction { get; }

        /*******************************************************************/
        public OptativeReaction(Card card, GameConditionWith<T> condition, GameCommand<T> logic, PlayActionType playActionType, GameActionTime time, string code, params string[] localizableArgs)
            : base(condition, logic, time)
        {
            Card = card;
            PlayAction = playActionType;
            LocalizableCode = code;
            LocalizableArgs = localizableArgs;
        }
    }
}
