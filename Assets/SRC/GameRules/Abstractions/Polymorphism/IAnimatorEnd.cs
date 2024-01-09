using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public interface IAnimatorEnd
    {
        Task CheckingAtEnd(GameAction gameAction);
    }
}
