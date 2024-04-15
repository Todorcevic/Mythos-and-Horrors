using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResolveChallengeGameAction : GameAction
    {
        public ChallengePhaseGameAction ChallengePhaseGameAction { get; init; }

        /*******************************************************************/
        public ResolveChallengeGameAction(ChallengePhaseGameAction challengePhaseGameAction)
        {
            ChallengePhaseGameAction = challengePhaseGameAction;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if ((bool)ChallengePhaseGameAction.IsSuccessful && ChallengePhaseGameAction.SuccesEffects.Count > 0)
            {
                foreach (Func<Task> succesEffect in ChallengePhaseGameAction.SuccesEffects)
                {
                    await succesEffect?.Invoke();
                }
            }
            else if (!(bool)ChallengePhaseGameAction.IsSuccessful && ChallengePhaseGameAction.FailEffects.Count > 0)
            {
                foreach (Func<Task> failEffect in ChallengePhaseGameAction.FailEffects)
                {
                    await failEffect?.Invoke();
                }
            }
        }
    }
}
