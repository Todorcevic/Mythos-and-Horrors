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
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;

        /*******************************************************************/
        async Task IPresenter<ChallengePhaseGameAction>.PlayAnimationWith(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (challengePhaseGameAction.IsUndo) await _challengeComponent.Hide();
            else
            {
                if (challengePhaseGameAction.ResultChallenge != null)
                {
                    await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
                    await PauseToContinue();
                    await _challengeComponent.Hide();
                }
                else
                {
                    Transform worldObject = challengePhaseGameAction.CardToChallenge == null ? null :
                      _cardViewsManager.GetCardView(challengePhaseGameAction.CardToChallenge).transform;
                    await _challengeComponent.Show(worldObject, challengePhaseGameAction.ActiveInvestigator)
                        .Join(_challengeComponent.UpdateInfo()).AsyncWaitForCompletion();
                }
            }
        }

        private async Task PauseToContinue()
        {
            _mainButtonComponent.SetEffect(new BaseEffect(null, null, PlayActionType.None, null, new Localization("MainButton_Continue")));
            _showSelectorComponent.MainButtonWaitingToContinueShowUp();
            await _clickHandler.WaitingClick();
            _showSelectorComponent.MainButtonWaitingToContinueHideUp();
        }

        async Task IPresenter<CommitCardsChallengeGameAction>.PlayAnimationWith(CommitCardsChallengeGameAction commitCardsChallengeGameAction)
        {
            await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
        }

        async Task IPresenter<RevealChallengeTokenGameAction>.PlayAnimationWith(RevealChallengeTokenGameAction revealChallengeTokenGA)
        {
            await _challengeBagComponent.DropToken(revealChallengeTokenGA.ChallengeTokenRevealed).AsyncWaitForCompletion();
            await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
        }

        async Task IPresenter<ResolveSingleChallengeTokenGameAction>.PlayAnimationWith(ResolveSingleChallengeTokenGameAction resolveSingleChallengeGA)
        {
            await _challengeBagComponent.ShakeToken(resolveSingleChallengeGA.ChallengeTokenToResolve).AsyncWaitForCompletion();
        }

        async Task IPresenter<RestoreChallengeTokenGameAction>.PlayAnimationWith(RestoreChallengeTokenGameAction restoreChallengeToken)
        {
            await _challengeBagComponent.RestoreToken(restoreChallengeToken.ChallengeTokenToRestore).AsyncWaitForCompletion();
            await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
        }
    }
}
