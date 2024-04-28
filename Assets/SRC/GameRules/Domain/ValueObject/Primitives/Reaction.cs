using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{

    public class Reaction<T> where T : GameAction
    {
        public Func<T, bool> Condition { get; init; }
        public Func<T, Task> Logic { get; init; }

        /*******************************************************************/
        public Reaction(Func<T, bool> reaction, Func<T, Task> logic)
        {
            Condition = reaction;
            Logic = logic;
        }

        /*******************************************************************/
        public async Task CheckToReact(GameAction gameAction)
        {
            if (gameAction is T action && Condition.Invoke(action)) await Logic.Invoke(action);
        }
    }
}
