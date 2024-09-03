using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IReaction : IAbility
    {
        GameActionTime Time { get; }
        bool Check(GameAction gameAction, GameActionTime time);
        Task React(GameAction gameAction);
    }
}
