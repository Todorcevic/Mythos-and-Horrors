using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules.News
{
    public class OptativeReaction<T> : Reaction<T>, Triggered where T : GameAction
    {
        public Card Card { get; }
        public PlayActionType PlayAction { get; }

        /*******************************************************************/
        public OptativeReaction(Card card, GameConditionWith<T> condition, GameCommand<T> logic, PlayActionType playActionType, GameActionTime time)
            : base(condition, logic, time)
        {
            Card = card;
            PlayAction = playActionType;
        }
    }
}
