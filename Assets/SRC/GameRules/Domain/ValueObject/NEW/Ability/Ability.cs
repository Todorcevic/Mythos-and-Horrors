using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules.News
{

    public class Ability
    {
        public Func<Task> GameInteraction { get; }
    }
}
