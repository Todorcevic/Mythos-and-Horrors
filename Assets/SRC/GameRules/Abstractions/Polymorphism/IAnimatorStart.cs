using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IAnimatorStart
    {
        Task CheckingAtStart(GameAction gameAction);
    }
}
