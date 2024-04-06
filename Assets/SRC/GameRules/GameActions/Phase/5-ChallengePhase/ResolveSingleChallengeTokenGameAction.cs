using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveSingleChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ResolveSingleChallengeTokenGameAction> _solveTokenPresenter;

        public ChallengeToken ChallengeTokenToResolve { get; private set; }

        public override bool CanBeExecuted => ChallengeTokenToResolve.Effect != null;

        /*******************************************************************/
        public ResolveSingleChallengeTokenGameAction(ChallengeToken challengeTokenToResolve)
        {
            ChallengeTokenToResolve = challengeTokenToResolve;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _solveTokenPresenter.PlayAnimationWith(this);
            await ChallengeTokenToResolve.Effect?.Invoke();
        }
    }
}
