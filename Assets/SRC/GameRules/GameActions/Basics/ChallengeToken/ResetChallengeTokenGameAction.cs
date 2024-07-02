using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResetChallengeTokenGameAction : GameAction
    {
        public ChallengeToken ChallengeToken { get; private set; }

        /*******************************************************************/
        public ResetChallengeTokenGameAction SetWith(ChallengeToken challengeToken)
        {
            ChallengeToken = challengeToken;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<UpdateChallengeTokenGameAction>()
                .SetWith(ChallengeToken, ChallengeToken.InitialValue, ChallengeToken.InititalEffect).Start();
        }
    }
}

