using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResultChallengeGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ResultChallengeGameAction> _resolveChallengePresenter;

        public ChallengePhaseGameAction ChallengePhaseGameAction { get; init; }
        public bool IsSuccessful { get; private set; }

        /*******************************************************************/
        public ResultChallengeGameAction(ChallengePhaseGameAction challengePhaseGameAction)
        {
            ChallengePhaseGameAction = challengePhaseGameAction;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (ChallengePhaseGameAction.IsAutoSucceed) IsSuccessful = true;
            else if (ChallengePhaseGameAction.IsAutoFail) IsSuccessful = false;
            else IsSuccessful = ChallengePhaseGameAction.TotalChallengeValue >= ChallengePhaseGameAction.DifficultValue;
            await _resolveChallengePresenter.PlayAnimationWith(this);
        }
    }
}
