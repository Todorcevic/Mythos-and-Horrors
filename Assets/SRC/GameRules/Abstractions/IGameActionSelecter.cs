using System.Threading.Tasks;

namespace GameRules
{
    public interface IGameActionSelecter
    {
        Task ShowThisActions(params GameAction[] gameActions);
    }
}
