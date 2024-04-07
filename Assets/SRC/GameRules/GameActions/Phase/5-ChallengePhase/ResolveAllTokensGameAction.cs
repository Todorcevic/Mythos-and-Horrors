using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveAllTokensGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        ChallengeToken TokenToResolve { get; init; }
        public override bool CanBeExecuted => TokenToResolve != null;

        /*******************************************************************/
        public ResolveAllTokensGameAction(ChallengeToken tokenToResolve)
        {
            TokenToResolve = tokenToResolve;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new ResolveSingleChallengeTokenGameAction(TokenToResolve));
            await _gameActionsProvider.Create(new ResolveAllTokensGameAction(_challengeTokensProvider.ChallengeTokensRevealed.NextElementFor(TokenToResolve)));
        }
    }
}
