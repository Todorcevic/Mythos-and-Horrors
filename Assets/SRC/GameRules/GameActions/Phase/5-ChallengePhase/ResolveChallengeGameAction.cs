﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResolveChallengeGameAction : GameAction
    {
        public ChallengePhaseGameAction ChallengePhaseGameAction { get; private set; }

        /*******************************************************************/
        public ResolveChallengeGameAction SetWith(ChallengePhaseGameAction challengePhaseGameAction)
        {
            ChallengePhaseGameAction = challengePhaseGameAction;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await ResolveTalentsCards();

            if (ChallengePhaseGameAction.IsSucceed && ChallengePhaseGameAction.SuccesEffects.Any())
            {
                await _gameActionsProvider.Create<SafeForeach<Func<Task>>>().SetWith(AllSuccessEffects, ExecuteEffect).Execute();

                IEnumerable<Func<Task>> AllSuccessEffects() => ChallengePhaseGameAction.SuccesEffects;
                async Task ExecuteEffect(Func<Task> effect) => await effect?.Invoke();
            }
            else if (!ChallengePhaseGameAction.IsSucceed && ChallengePhaseGameAction.FailEffects.Any())
            {
                await _gameActionsProvider.Create<SafeForeach<Func<Task>>>().SetWith(AllFailEffects, ExecuteEffect).Execute();

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
