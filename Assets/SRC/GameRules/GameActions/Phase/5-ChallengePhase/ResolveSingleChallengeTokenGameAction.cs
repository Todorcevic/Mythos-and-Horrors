using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveSingleChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ResolveSingleChallengeTokenGameAction> _solveTokenPresenter;

        public ChallengeToken ChallengeTokenToResolve { get; private set; }
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public ResolveSingleChallengeTokenGameAction(ChallengeToken challengeTokenToResolve, Investigator investigator)
        {
            ChallengeTokenToResolve = challengeTokenToResolve;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (ChallengeTokenToResolve.Effect != null)
            {
                await _solveTokenPresenter.PlayAnimationWith(this);
                await ChallengeTokenToResolve.Effect?.Invoke(Investigator);
            }
        }
    }
}
