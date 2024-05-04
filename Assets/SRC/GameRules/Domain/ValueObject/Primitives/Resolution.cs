using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Resolution
    {
        public History History { get; private set; }
        public Func<Task> Logic { get; private set; }

        /*******************************************************************/
        public Resolution(History history, Func<Task> logic)
        {
            History = history;
            Logic = logic;
        }
    }
}
