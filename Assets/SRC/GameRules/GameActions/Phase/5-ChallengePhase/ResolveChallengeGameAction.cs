using ModestTree.Util;
using System;
using System.Collections.Generic;
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
                await new SafeForeach<Func<Task>>(ExecuteEffect, AllSuccessEffects).Execute();

                async Task ExecuteEffect(Func<Task> effect) => await effect?.Invoke();
                IEnumerable<Func<Task>> AllSuccessEffects() => ChallengePhaseGameAction.SuccesEffects;
            }
            else if (!(bool)ChallengePhaseGameAction.IsSuccessful && ChallengePhaseGameAction.FailEffects.Count > 0)
            {
                await new SafeForeach<Func<Task>>(ExecuteEffect, AllFailEffects).Execute();

                async Task ExecuteEffect(Func<Task> effect) => await effect?.Invoke();
                IEnumerable<Func<Task>> AllFailEffects() => ChallengePhaseGameAction.FailEffects;
            }
        }
    }
}
