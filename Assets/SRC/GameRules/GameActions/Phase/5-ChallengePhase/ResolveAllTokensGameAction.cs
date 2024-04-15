using System.Collections.Generic;
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
            await new SafeForeach<ChallengeToken>(Resolve, GetTokensToResolve).Execute();
        }

        /*******************************************************************/
        private IEnumerable<ChallengeToken> GetTokensToResolve() => _challengeTokensProvider.ChallengeTokensRevealed;

        private async Task Resolve(ChallengeToken challengeToken) =>
            await _gameActionsProvider.Create(new ResolveSingleChallengeTokenGameAction(challengeToken));
    }
}
