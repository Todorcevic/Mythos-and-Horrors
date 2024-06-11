using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class UpdateChallengeTokenGameAction : GameAction
    {
        public Func<Investigator, int> _OldValue;
        public Func<Investigator, Task> _OldEffect;

        public ChallengeToken ChallengeToken { get; }
        public Func<Investigator, int> NewValue { get; }
        public Func<Investigator, Task> NewEffect { get; }

        /*******************************************************************/
        public UpdateChallengeTokenGameAction(ChallengeToken challengeToken)
        {
            ChallengeToken = challengeToken;
        }

        public UpdateChallengeTokenGameAction(ChallengeToken challengeToken, Func<Investigator, int> newValue, Func<Investigator, Task> newEffect)
        {
            ChallengeToken = challengeToken;
            NewValue = newValue;
            NewEffect = newEffect;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _OldValue = ChallengeToken.Value;
            _OldEffect = ChallengeToken.Effect;
            ChallengeToken.ResetToken();
            if (NewValue != null) ChallengeToken.UpdateValue(NewValue);
            if (NewEffect != null) ChallengeToken.UpdateEffect(NewEffect);
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

