using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class UpdateChallengeTokenGameAction : GameAction
    {
        public Func<Investigator, int> _OldValue;
        public Func<Investigator, Task> _OldEffect;

        public ChallengeToken ChallengeToken { get; private set; }
        public Func<Investigator, int> NewValue { get; private set; }
        public Func<Investigator, Task> NewEffect { get; private set; }

        /*******************************************************************/
        public UpdateChallengeTokenGameAction SetWith(ChallengeToken challengeToken, Func<Investigator, int> newValue, Func<Investigator, Task> newEffect)
        {
            ChallengeToken = challengeToken;
            NewValue = newValue;
            NewEffect = newEffect;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _OldValue = ChallengeToken.Value;
            _OldEffect = ChallengeToken.Effect;
            ChallengeToken.UpdateValue(NewValue);
            ChallengeToken.UpdateEffect(NewEffect);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            ChallengeToken.UpdateValue(_OldValue);
            ChallengeToken.UpdateEffect(_OldEffect);
            await base.Undo();
        }
    }
}

