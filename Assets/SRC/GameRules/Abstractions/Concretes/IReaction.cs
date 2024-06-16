using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IReaction
    {
        GameActionTime Time { get; }
        Task React(GameAction gameAction);
        void Disable();
        void Enable();
    }
}
