﻿using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengePresenter : IPresenter<ChallengePhaseGameAction>, IPresenter<CommitCardsChallengeGameAction>,
        IPresenter<FinishChallengeGameAction>, IPresenter<RevealChallengeTokenGameAction>, IPresenter<ResolveSingleChallengeTokenGameAction>,
        IPresenter<ResultChallengeGameAction>, IPresenter<RestoreChallengeTokens>
    {
        [Inject] private readonly ChallengeComponent _challengeComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ChallengeBagComponent _challengeBagComponent;

        /*******************************************************************/
        async Task IPresenter<ChallengePhaseGameAction>.PlayAnimationWith(ChallengePhaseGameAction challengePhaseGameAction)
        {
            Transform initialPosition = challengePhaseGameAction.CardToChallenge == null ?
                null :
                _cardViewsManager.GetCardView(challengePhaseGameAction.CardToChallenge).transform;

            await _challengeComponent.Show(challengePhaseGameAction, initialPosition);
        }

        async Task IPresenter<CommitCardsChallengeGameAction>.PlayAnimationWith(CommitCardsChallengeGameAction commintCardChallengeGameAction)
        {
            _challengeComponent.UpdateInfo();
            await Task.CompletedTask;
        }

        async Task IPresenter<FinishChallengeGameAction>.PlayAnimationWith(FinishChallengeGameAction _)
        {
            await _challengeComponent.Hide();
        }

        async Task IPresenter<RevealChallengeTokenGameAction>.PlayAnimationWith(RevealChallengeTokenGameAction revealChallengeTokenGA)
        {
            await _challengeBagComponent.DropToken(revealChallengeTokenGA.ChallengeTokenRevealed);
            _challengeComponent.SetToken(revealChallengeTokenGA.ChallengeTokenRevealed);
        }

        async Task IPresenter<ResolveSingleChallengeTokenGameAction>.PlayAnimationWith(ResolveSingleChallengeTokenGameAction resolveSingleChallengeGA)
        {
            //await _challengeBagComponent.ShowCenter(resolveSingleChallengeGA.ChallengeTokenToResolve).AsyncWaitForCompletion();

            await _challengeComponent.ShowSceneToken(resolveSingleChallengeGA.ChallengeTokenToResolve).AsyncWaitForCompletion();
        }

        async Task IPresenter<ResultChallengeGameAction>.PlayAnimationWith(ResultChallengeGameAction resultChallengeGameAction)
        {
            _challengeComponent.UpdateResult(resultChallengeGameAction.IsSuccessful);
            _challengeComponent.UpdateInfo();
            await Task.CompletedTask;
        }

        async Task IPresenter<RestoreChallengeTokens>.PlayAnimationWith(RestoreChallengeTokens gameAction)
        {
            await _challengeBagComponent.RestoreAllTokens().AsyncWaitForCompletion();
        }
    }
}