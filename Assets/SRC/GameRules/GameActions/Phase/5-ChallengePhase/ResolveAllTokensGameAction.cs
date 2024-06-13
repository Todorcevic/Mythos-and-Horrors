using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveAllTokensGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public ResolveAllTokensGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new SafeForeach<ChallengeToken>(AllTokensRevealed, Resolve));

            /*******************************************************************/

            IEnumerable<ChallengeToken> AllTokensRevealed() => _challengeTokensProvider.ChallengeTokensRevealed;

            async Task Resolve(ChallengeToken challengeToken) =>
                await _gameActionsProvider.Create(new ResolveSingleChallengeTokenGameAction(challengeToken, Investigator));
        }
    }
}
