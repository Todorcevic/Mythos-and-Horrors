using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResetChallengeTokenGameAction : UpdateChallengeTokenGameAction
    {
        public ResetChallengeTokenGameAction SetWith(ChallengeToken challengeToken)
        {
            SetWith(challengeToken, challengeToken.Value, challengeToken.Effect);
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ChallengeToken.ResetToken();
            await Task.CompletedTask;
        }
    }
}

