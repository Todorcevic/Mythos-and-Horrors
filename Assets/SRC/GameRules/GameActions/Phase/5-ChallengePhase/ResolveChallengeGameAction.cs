using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResolveChallengeGameAction : GameAction
    {
        public Func<Task> SuccesEffect { get; init; }
        public Func<Task> FailEffect { get; init; }
        public bool IsSuccessful { get; init; }

        /*******************************************************************/
        public ResolveChallengeGameAction(bool isSuccessful, Func<Task> succesEffect, Func<Task> failEffect)
        {
            IsSuccessful = isSuccessful;
            SuccesEffect = succesEffect;
            FailEffect = failEffect;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (IsSuccessful)
            {
                if (SuccesEffect != null) await SuccesEffect.Invoke();
            }
            else if (FailEffect != null) await FailEffect.Invoke();
        }
    }
}
