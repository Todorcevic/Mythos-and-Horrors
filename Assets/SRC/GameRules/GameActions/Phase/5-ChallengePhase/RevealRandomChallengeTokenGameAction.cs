using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RevealRandomChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public ChallengeToken ChallengeTokenRevealed { get; private set; }
        public Investigator Investigator { get; }

        /*******************************************************************/
        public RevealRandomChallengeTokenGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ChallengeTokenRevealed = _challengeTokensProvider.GetRandomToken();
            await _gameActionsProvider.Create(new RevealChallengeTokenGameAction(ChallengeTokenRevealed, Investigator));
        }
    }
}
