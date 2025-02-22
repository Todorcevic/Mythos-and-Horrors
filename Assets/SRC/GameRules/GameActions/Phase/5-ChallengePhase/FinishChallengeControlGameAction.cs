using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class FinishChallengeControlGameAction : GameAction
    {
        public ChallengePhaseGameAction ChallengePhaseGameAction { get; private set; }

        /*******************************************************************/
        public FinishChallengeControlGameAction SetWith(ChallengePhaseGameAction challengePhaseGameAction)
        {
            ChallengePhaseGameAction = challengePhaseGameAction;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await Task.CompletedTask;
        }
    }
}
