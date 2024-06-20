using System;
using System.Collections.Generic;
using System.Linq;
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
            await ResolveTalentsCards();

            if ((bool)ChallengePhaseGameAction.IsSuccessful && ChallengePhaseGameAction.SuccesEffects.Any())
            {
                await _gameActionsProvider.Create(new SafeForeach<Func<Task>>(AllSuccessEffects, ExecuteEffect));

                IEnumerable<Func<Task>> AllSuccessEffects() => ChallengePhaseGameAction.SuccesEffects;
                async Task ExecuteEffect(Func<Task> effect) => await effect?.Invoke();
            }
            else if (!(bool)ChallengePhaseGameAction.IsSuccessful && ChallengePhaseGameAction.FailEffects.Any())
            {
                await _gameActionsProvider.Create(new SafeForeach<Func<Task>>(AllFailEffects, ExecuteEffect));

                IEnumerable<Func<Task>> AllFailEffects() => ChallengePhaseGameAction.FailEffects;
                async Task ExecuteEffect(Func<Task> effect) => await effect?.Invoke();
            }
        }

        private async Task ResolveTalentsCards()
        {
            foreach (CardTalent cardTalent in ChallengePhaseGameAction.CurrentCommitsCards.OfType<CardTalent>()
                .Where(talent => talent.TalentCondition(ChallengePhaseGameAction)))
            {
                await cardTalent.TalentLogic(ChallengePhaseGameAction);
            }
        }
    }
}
