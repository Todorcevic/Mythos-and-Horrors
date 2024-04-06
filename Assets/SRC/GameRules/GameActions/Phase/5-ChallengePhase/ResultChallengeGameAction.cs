using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResultChallengeGameAction : GameAction
    {
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
            await Task.CompletedTask;
        }
    }
}
