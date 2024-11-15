﻿namespace MythosAndHorrors.GameRules
{
    public class OptativeReaction<T> : Reaction<T>, ITriggered where T : GameAction
    {
        public Card Card { get; }
        public PlayActionType PlayAction { get; }

        /*******************************************************************/
        public OptativeReaction(Card card, GameConditionWith<T> condition, GameCommand<T> logic, PlayActionType playActionType, GameActionTime time, Localization localization)
            : base(condition, logic, time)
        {
            Card = card;
            PlayAction = playActionType;
            Localization = localization;
        }
    }
}
