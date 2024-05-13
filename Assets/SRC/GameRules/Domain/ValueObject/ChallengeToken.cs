using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ChallengeToken
    {
        public ChallengeTokenType TokenType { get; }
        public Func<Investigator, int> Value { get; }
        public Func<Investigator, Task> Effect { get; }
        public string Description { get; }

        /*******************************************************************/
        public ChallengeToken(ChallengeTokenType type, Func<Investigator, int> value = null, Func<Investigator, Task> effect = null, string description = null)
        {
            TokenType = type;
            Value = value ?? ((_) => 0);
            Effect = effect;
            Description = description;
        }
    }
}
