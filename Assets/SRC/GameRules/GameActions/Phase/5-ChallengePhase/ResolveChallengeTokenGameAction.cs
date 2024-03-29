using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ResolveChallengeTokenGameAction> _solveTokenPresenter;

        public ChallengeToken ChallengeTokenToResolve { get; private set; }

        /*******************************************************************/
        public ResolveChallengeTokenGameAction(ChallengeToken challengeTokenToResolve)
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
