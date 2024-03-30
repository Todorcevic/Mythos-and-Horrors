using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveSingleChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ResolveSingleChallengeTokenGameAction> _solveTokenPresenter;

        public ChallengeToken ChallengeTokenToResolve { get; private set; }

        /*******************************************************************/
        public ResolveSingleChallengeTokenGameAction(ChallengeToken challengeTokenToResolve)
        {
            ChallengeTokenToResolve = challengeTokenToResolve;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _solveTokenPresenter.PlayAnimationWith(this); //Mover a la ventana UI
            await ChallengeTokenToResolve.Effect?.Invoke();
        }
    }
}
