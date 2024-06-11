using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ChallengeToken
    {
        public Func<Investigator, int> _initialValue;
        public Func<Investigator, Task> inititalEffect;

        public ChallengeTokenType TokenType { get; }
        public Func<Investigator, int> Value { get; private set; }
        public Func<Investigator, Task> Effect { get; private set; }
        public string Description { get; }

        /*******************************************************************/
        public ChallengeToken(ChallengeTokenType type, Func<Investigator, int> value = null, Func<Investigator, Task> effect = null, string description = null)
        {
            TokenType = type;
            _initialValue = Value = value ?? ((_) => 0);
            inititalEffect = Effect = effect;
            Description = description;
        }
        /*******************************************************************/
        public void UpdateValue(Func<Investigator, int> newValue)
        {
            Value = newValue;
        }

        public void UpdateEffect(Func<Investigator, Task> newEffect)
        {
            Effect = newEffect;
        }

        public void ResetToken()
        {
            Value = _initialValue;
            Effect = inititalEffect;
        }
    }
}
