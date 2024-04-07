using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengePresenter : IPresenter<ChallengePhaseGameAction>, IPresenter<CommitCardsChallengeGameAction>,
        IPresenter<RevealChallengeTokenGameAction>, IPresenter<ResolveSingleChallengeTokenGameAction>,
        IPresenter<RestoreChallengeToken>, IPresenter<ResultChallengeGameAction>
    {
        [Inject] private readonly ChallengeComponent _challengeComponent;
        [Inject] private readonly ChallengeBagComponent _challengeBagComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        async Task IPresenter<ChallengePhaseGameAction>.PlayAnimationWith(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction.IsUndo) await _challengeComponent.Hide();
            else
            {
                await _challengeComponent.UpdateInfo(challengePhaseGameAction).AsyncWaitForCompletion();
                Transform worldObject = challengePhaseGameAction.CardToChallenge == null ? null :
                     _cardViewsManager.GetCardView(challengePhaseGameAction.CardToChallenge).transform;
                await _challengeComponent.Show(worldObject);
            }
        }

        async Task IPresenter<CommitCardsChallengeGameAction>.PlayAnimationWith(CommitCardsChallengeGameAction commitCardsChallengeGameAction)
        {
            await _challengeComponent.UpdateInfo(_gameActionsProvider.CurrentChallenge).AsyncWaitForCompletion();
        }

        async Task IPresenter<ResultChallengeGameAction>.PlayAnimationWith(ResultChallengeGameAction resultChallengeGameAction)
        {
            await _challengeComponent.UpdateInfo(resultChallengeGameAction.ChallengePhaseGameAction).AsyncWaitForCompletion();
            await Task.Delay(1000);
            await _challengeComponent.Hide();
        }

        async Task IPresenter<RevealChallengeTokenGameAction>.PlayAnimationWith(RevealChallengeTokenGameAction revealChallengeTokenGA)
        {
            await _challengeBagComponent.DropToken(revealChallengeTokenGA.ChallengeTokenRevealed);
            await _challengeComponent.UpdateInfo(_gameActionsProvider.CurrentChallenge).AsyncWaitForCompletion();
            _challengeComponent.SetToken(revealChallengeTokenGA.ChallengeTokenRevealed);
        }

        async Task IPresenter<ResolveSingleChallengeTokenGameAction>.PlayAnimationWith(ResolveSingleChallengeTokenGameAction resolveSingleChallengeGA)
        {
            await _challengeBagComponent.ShakeToken(resolveSingleChallengeGA.ChallengeTokenToResolve).AsyncWaitForCompletion();
        }

        async Task IPresenter<RestoreChallengeToken>.PlayAnimationWith(RestoreChallengeToken restoreChallengeToken)
        {
            await _challengeBagComponent.RestoreToken(restoreChallengeToken.ChallengeTokenToRestore).AsyncWaitForCompletion();
        }
    }
}
