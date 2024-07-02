using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RevealRandomChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public ChallengeToken ChallengeTokenRevealed { get; private set; }
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public RevealRandomChallengeTokenGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ChallengeTokenRevealed = _challengeTokensProvider.GetRandomToken();
            await _gameActionsProvider.Create<RevealChallengeTokenGameAction>().SetWith(ChallengeTokenRevealed, Investigator).Start();
        }
    }
}
