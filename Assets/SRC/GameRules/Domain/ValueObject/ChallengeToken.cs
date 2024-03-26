using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ChallengeToken
    {
        public Func<int> Value { get; }
        public Func<Task> Effect { get; }

        /*******************************************************************/
        public ChallengeToken(Func<int> value = null, Func<Task> effect = null)
        {
            Value = value ?? (() => 0);
            Effect = effect ?? (() => Task.CompletedTask);
        }
    }
}
