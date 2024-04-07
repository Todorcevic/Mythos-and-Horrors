using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RevealRandomChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public ChallengeToken ChallengeTokenRevealed { get; private set; }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ChallengeTokenRevealed = _challengeTokensProvider.GetRandomToken();
            await _gameActionsProvider.Create(new RevealChallengeTokenGameAction(ChallengeTokenRevealed));
        }
    }
}
