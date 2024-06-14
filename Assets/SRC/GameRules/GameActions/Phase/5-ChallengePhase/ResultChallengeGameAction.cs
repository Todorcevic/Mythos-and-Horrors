using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResultChallengeGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public bool? IsSuccessful { get; set; }
        public int TotalDifferenceValue { get; set; }
        public ChallengePhaseGameAction ChallengePhaseGameAction { get; init; }
        public List<ChallengeToken> TokensRevealed { get; private set; }

        /*******************************************************************/
        public ResultChallengeGameAction(ChallengePhaseGameAction challengePhaseGameAction)
        {
            ChallengePhaseGameAction = challengePhaseGameAction;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            TokensRevealed = _challengeTokensProvider.ChallengeTokensRevealed.ToList();
            if (ChallengePhaseGameAction.IsAutoSucceed) IsSuccessful = true;
            else if (ChallengePhaseGameAction.IsAutoFail) IsSuccessful = false;
            else IsSuccessful = ChallengePhaseGameAction.CurrentTotalChallengeValue >= ChallengePhaseGameAction.DifficultValue;
            TotalDifferenceValue = ChallengePhaseGameAction.CurrentTotalChallengeValue - ChallengePhaseGameAction.DifficultValue;
            await Task.CompletedTask;
        }
    }
}
