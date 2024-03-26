using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ChallengeToken
    {
        public ChallengeTokenType TokenType { get; }
        public Func<int> Value { get; }
        public Func<Task> Effect { get; }

        /*******************************************************************/
        public ChallengeToken(ChallengeTokenType type, Func<int> value = null, Func<Task> effect = null)
        {
            TokenType = type;
            Value = value ?? (() => 0);
            Effect = effect ?? (() => Task.CompletedTask);

        }
    }
}
