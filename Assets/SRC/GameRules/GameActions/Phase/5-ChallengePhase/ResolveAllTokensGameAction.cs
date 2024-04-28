using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveAllTokensGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new SafeForeach<ChallengeToken>(_challengeTokensProvider.ChallengeTokensRevealed, Resolve));
        }

        /*******************************************************************/
        private async Task Resolve(ChallengeToken challengeToken) =>
            await _gameActionsProvider.Create(new ResolveSingleChallengeTokenGameAction(challengeToken));
    }
}
