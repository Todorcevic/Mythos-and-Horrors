using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestoreChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public ChallengeToken ChallengeTokenToRestore { get; private set; }

        /*******************************************************************/
        public RestoreChallengeTokenGameAction SetWith(ChallengeToken challengeTokenToRestore)
        {
            ChallengeTokenToRestore = challengeTokenToRestore;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _challengeTokensProvider.RestoreSingleToken(ChallengeTokenToRestore);
            await Task.CompletedTask;
        }
    }
}
