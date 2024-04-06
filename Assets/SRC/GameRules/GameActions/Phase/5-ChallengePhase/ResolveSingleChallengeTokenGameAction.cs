using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveSingleChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ResolveSingleChallengeTokenGameAction> _solveTokenPresenter;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public ChallengeToken ChallengeTokenToResolve { get; private set; }

        /*******************************************************************/
        public ResolveSingleChallengeTokenGameAction(ChallengeToken challengeTokenToResolve)
        {
            ChallengeTokenToResolve = challengeTokenToResolve;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (ChallengeTokenToResolve.Effect != null)
            {
                await _solveTokenPresenter.PlayAnimationWith(this);
                await ChallengeTokenToResolve.Effect?.Invoke();
            }
            await _gameActionsProvider.Create(new RestoreChallengeToken(ChallengeTokenToResolve));
        }
    }
}
