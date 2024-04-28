using System;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveChallengeGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
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
                await _gameActionsProvider.Create(new SafeForeach<Func<Task>>(ChallengePhaseGameAction.SuccesEffects, ExecuteEffect));

                async Task ExecuteEffect(Func<Task> effect) => await effect?.Invoke();
            }
            else if (!(bool)ChallengePhaseGameAction.IsSuccessful && ChallengePhaseGameAction.FailEffects.Count > 0)
            {
                await _gameActionsProvider.Create(new SafeForeach<Func<Task>>(ChallengePhaseGameAction.FailEffects, ExecuteEffect));

                async Task ExecuteEffect(Func<Task> effect) => await effect?.Invoke();
            }
        }
    }
}
