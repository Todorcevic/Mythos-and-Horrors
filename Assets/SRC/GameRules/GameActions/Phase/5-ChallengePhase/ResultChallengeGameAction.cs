using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResultChallengeGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ResultChallengeGameAction> _resultChallengePresent;

        public ChallengePhaseGameAction ChallengePhaseGameAction { get; init; }

        /*******************************************************************/
        public ResultChallengeGameAction(ChallengePhaseGameAction challengePhaseGameAction)
        {
            ChallengePhaseGameAction = challengePhaseGameAction;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (ChallengePhaseGameAction.IsAutoSucceed) ChallengePhaseGameAction.IsSuccessful = true;
            else if (ChallengePhaseGameAction.IsAutoFail) ChallengePhaseGameAction.IsSuccessful = false;
            else ChallengePhaseGameAction.IsSuccessful = ChallengePhaseGameAction.TotalChallengeValue >= ChallengePhaseGameAction.DifficultValue;
            await _resultChallengePresent.PlayAnimationWith(this);
        }
    }
}
