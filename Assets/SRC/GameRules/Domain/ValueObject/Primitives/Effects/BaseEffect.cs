using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record BaseEffect
    {
        public Func<Task> Logic { get; private set; }

        /*******************************************************************/
        public BaseEffect(Func<Task> logic)
        {
            Logic = logic;
        }

        /*******************************************************************/
    }
}
