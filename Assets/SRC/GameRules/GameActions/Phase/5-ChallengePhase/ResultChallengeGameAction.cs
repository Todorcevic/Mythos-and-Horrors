using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResultChallengeGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ResultChallengeGameAction> _resolveChallengePresenter;

        public int TotalChallenge { get; init; }
        public int DifficultValue { get; init; }
        public bool IsSuccessful { get; private set; }

        /*******************************************************************/
        public ResultChallengeGameAction(int totalChallengeValue, int difficultValue)
        {
            TotalChallenge = totalChallengeValue;
            DifficultValue = difficultValue;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            IsSuccessful = TotalChallenge >= DifficultValue;
            await _resolveChallengePresenter.PlayAnimationWith(this);
        }
    }
}
