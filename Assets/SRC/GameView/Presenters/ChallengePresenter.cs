using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChallengePresenter
    {
        [Inject] private readonly ChallengeComponent _challengeComponent;
        [Inject] private readonly ChallengeBagComponent _challengeBagComponent;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;
        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;

        /*******************************************************************/
        public async Task ShowChallenge(ChallengePhaseGameAction challengePhaseGameAction)
        {
            Transform worldObject = challengePhaseGameAction.CardToChallenge == null ? null :
              _cardViewsManager.GetCardView(challengePhaseGameAction.CardToChallenge).transform;
            await _challengeComponent.Show(worldObject, challengePhaseGameAction.ActiveInvestigator)
                .Join(_challengeComponent.UpdateInfo()).AsyncWaitForCompletion();
        }

        public async Task FinalizeChallenge()
        {
            await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
            await PauseToContinue();
            await HideChallenge();

            /*******************************************************************/
            async Task PauseToContinue()
            {
                _mainButtonComponent.SetEffect(new BaseEffect(null, null, PlayActionType.None, null, new Localization("MainButton_Continue")));
                _showSelectorComponent.MainButtonWaitingToContinueShowUp();
                await _clickHandler.WaitingClick();
                _showSelectorComponent.MainButtonWaitingToContinueHideUp();
            }
        }

        public async Task UpdateInfo() => await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();

        public async Task DropToken(ChallengeToken challengeTokenRevealed)
        {
            await _challengeBagComponent.DropToken(challengeTokenRevealed).AsyncWaitForCompletion();
            await _challengeComponent.UpdateInfo().AsyncWaitForCompletion();
        }

        public async Task SolveTokenAnimation(ChallengeToken challengeTokenSolve)
        {
            await _challengeBagComponent.ShakeToken(challengeTokenSolve).AsyncWaitForCompletion();
        }

        public async Task RestoreToken(ChallengeToken challengeTokenToRestore)
        {
            await _challengeBagComponent.RestoreToken(challengeTokenToRestore).AsyncWaitForCompletion();
        }

        public async Task HideChallenge() => await _challengeComponent.Hide().AsyncWaitForCompletion();
    }
}
