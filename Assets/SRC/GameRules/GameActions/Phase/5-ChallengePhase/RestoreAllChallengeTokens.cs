using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestoreAllChallengeTokens : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new SafeForeach<ChallengeToken>(_challengeTokensProvider.ChallengeTokensRevealed, Restore));
        }

        /*******************************************************************/
        private async Task Restore(ChallengeToken challengeToken) =>
            await _gameActionsProvider.Create(new RestoreChallengeToken(challengeToken));
    }
}
