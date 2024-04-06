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
        public ResolveChallengeGameAction(ChallengePhaseGameAction challengePhaseGameAction)
        {
            IsSuccessful = (bool)challengePhaseGameAction.IsSuccessful;
            SuccesEffect = challengePhaseGameAction.SuccesEffect;
            FailEffect = challengePhaseGameAction.FailEffect;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (IsSuccessful && SuccesEffect != null) await SuccesEffect.Invoke();
            else if (!IsSuccessful && FailEffect != null) await FailEffect.Invoke();
        }
    }
}
