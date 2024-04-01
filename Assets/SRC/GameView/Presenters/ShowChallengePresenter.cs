using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ShowChallengePresenter : IPresenter<ChallengePhaseGameAction>, IPresenter<CommitCardsChallengeGameAction>
    {
        [Inject] private readonly ChallengeComponent _challengeComponent;
        [Inject] private readonly CardViewsManager _cardViewsManager;

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
            await _challengeComponent.UpdateInfo(commintCardChallengeGameAction.CurrentChallenge);
        }
    }
}
