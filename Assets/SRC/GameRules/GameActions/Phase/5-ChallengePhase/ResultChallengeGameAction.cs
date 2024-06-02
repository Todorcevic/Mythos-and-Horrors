using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResultChallengeGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ResultChallengeGameAction> _resultChallengePresent;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public bool? IsSuccessful { get; set; }
        public int TotalDifferenceValue { get; set; }
        public ChallengePhaseGameAction ChallengePhaseGameAction { get; init; }

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
            TotalDifferenceValue = ChallengePhaseGameAction.TotalChallengeValue - ChallengePhaseGameAction.DifficultValue;
            await _resultChallengePresent.PlayAnimationWith(this);
        }
    }
}
