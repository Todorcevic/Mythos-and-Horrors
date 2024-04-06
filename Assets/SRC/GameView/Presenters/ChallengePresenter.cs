using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengePresenter : IPresenter<ChallengePhaseGameAction>, IPresenter<CommitCardsChallengeGameAction>,
        IPresenter<RevealChallengeTokenGameAction>, IPresenter<ResolveSingleChallengeTokenGameAction>,
        IPresenter<RestoreChallengeToken>
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

            await _challengeComponent.UpdateInfo(challengePhaseGameAction).AsyncWaitForCompletion();
            if (challengePhaseGameAction.IsFinished) await _challengeComponent.Hide();
            else await _challengeComponent.Show(initialPosition);
        }

        async Task IPresenter<CommitCardsChallengeGameAction>.PlayAnimationWith(CommitCardsChallengeGameAction commitCardsChallengeGameAction)
        {
            await _challengeComponent.UpdateInfo(commitCardsChallengeGameAction.ChallengePhase).AsyncWaitForCompletion();
        }

        async Task IPresenter<RevealChallengeTokenGameAction>.PlayAnimationWith(RevealChallengeTokenGameAction revealChallengeTokenGA)
        {
            await _challengeBagComponent.DropToken(revealChallengeTokenGA.ChallengeTokenRevealed);
            _challengeComponent.SetToken(revealChallengeTokenGA.ChallengeTokenRevealed);
            await _challengeComponent.UpdateInfo(revealChallengeTokenGA.ChallengePhase).AsyncWaitForCompletion();
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
