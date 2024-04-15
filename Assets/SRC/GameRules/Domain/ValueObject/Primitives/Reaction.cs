using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{


    public class Reaction
    {
        public Func<GameAction, bool> Condition { get; init; }
        public Func<Task> Logic { get; init; }

        /*******************************************************************/
        public Reaction(Func<GameAction, bool> reaction, Func<Task> logic)
        {
            Condition = reaction;
            Logic = logic;
        }

        /*******************************************************************/
        public async Task Check(GameAction gameAction)
        {
            if (Condition.Invoke(gameAction)) await Logic.Invoke();
        }
    }
}
