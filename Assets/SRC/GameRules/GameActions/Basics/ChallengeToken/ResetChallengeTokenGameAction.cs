using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ResetChallengeTokenGameAction : UpdateChallengeTokenGameAction
    {
        public ResetChallengeTokenGameAction(ChallengeToken challengeToken) : base(challengeToken, challengeToken.Value, challengeToken.Effect)
        { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ChallengeToken.ResetToken();
            await Task.CompletedTask;
        }
    }
}

