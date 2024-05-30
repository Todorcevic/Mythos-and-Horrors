using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IReaction
    {
        Task React(GameAction gameAction);
        void Disable();
        void Enable();
        void NewCondition(Func<GameAction, bool> condition);
    }
}
