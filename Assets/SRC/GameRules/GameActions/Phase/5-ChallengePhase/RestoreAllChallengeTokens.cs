using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestoreAllChallengeTokens : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        ChallengeToken TokenToRestore { get; init; }

        public override bool CanBeExecuted => TokenToRestore != null;

        /*******************************************************************/
        public RestoreAllChallengeTokens(ChallengeToken tokenToRestore)
        {
            TokenToRestore = tokenToRestore;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new RestoreChallengeToken(TokenToRestore));
            await _gameActionsProvider.Create(new RestoreAllChallengeTokens(_challengeTokensProvider.ChallengeTokensRevealed.NextElementFor(TokenToRestore)));
        }
    }
}
