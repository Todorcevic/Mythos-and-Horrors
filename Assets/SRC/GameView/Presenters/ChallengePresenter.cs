using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengePresenter : IPresenter<ChallengePhaseGameAction>, IPresenter<CommitCardsChallengeGameAction>,
        IPresenter<RevealChallengeTokenGameAction>, IPresenter<ResolveSingleChallengeTokenGameAction>,
        IPresenter<RestoreChallengeTokenGameAction>
    {
        [Inject] private readonly ChallengeComponent _challengeComponent;
        [Inject] private readonly ChallengeBagComponent _challengeBagComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;

        /*******************************************************************/
        async Task IPresenter<ChallengePhaseGameAction>.PlayAnimationWith(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction.IsUndo) await _challengeComponent.Hide();
            else
            {
                if (challengePhaseGameAction.ResultChallenge != null)
                {
                    await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
                    await _challengeComponent.Hide();
                }
                else
                {
                    await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
                    Transform worldObject = challengePhaseGameAction.CardToChallenge == null ? null :
                         _cardViewsManager.GetCardView(challengePhaseGameAction.CardToChallenge).transform;
                    await _challengeComponent.Show(worldObject);
                }
            }
        }

        async Task IPresenter<CommitCardsChallengeGameAction>.PlayAnimationWith(CommitCardsChallengeGameAction commitCardsChallengeGameAction)
        {
            await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
        }

        async Task IPresenter<RevealChallengeTokenGameAction>.PlayAnimationWith(RevealChallengeTokenGameAction revealChallengeTokenGA)
        {
            await _challengeBagComponent.DropToken(revealChallengeTokenGA.ChallengeTokenRevealed, revealChallengeTokenGA.Investigator).AsyncWaitForCompletion();
            await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
            _challengeComponent.SetToken(revealChallengeTokenGA.ChallengeTokenRevealed, revealChallengeTokenGA.Investigator);
        }

        async Task IPresenter<ResolveSingleChallengeTokenGameAction>.PlayAnimationWith(ResolveSingleChallengeTokenGameAction resolveSingleChallengeGA)
        {
            await _challengeBagComponent.ShakeToken(resolveSingleChallengeGA.ChallengeTokenToResolve).AsyncWaitForCompletion();
        }

        async Task IPresenter<RestoreChallengeTokenGameAction>.PlayAnimationWith(RestoreChallengeTokenGameAction restoreChallengeToken)
        {
            await _challengeBagComponent.RestoreToken(restoreChallengeToken.ChallengeTokenToRestore).AsyncWaitForCompletion();
            _challengeComponent.RestoreToken(restoreChallengeToken.ChallengeTokenToRestore);
        }
    }
}
