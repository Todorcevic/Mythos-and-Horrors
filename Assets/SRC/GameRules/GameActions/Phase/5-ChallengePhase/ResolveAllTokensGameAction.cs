using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ResolveAllTokensGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public ResolveAllTokensGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<SafeForeach<ChallengeToken>>().SetWith(AllTokensRevealed, Resolve).Start();

            /*******************************************************************/

            IEnumerable<ChallengeToken> AllTokensRevealed() => _challengeTokensProvider.ChallengeTokensRevealed;

            async Task Resolve(ChallengeToken challengeToken) =>
                await _gameActionsProvider.Create<ResolveSingleChallengeTokenGameAction>().SetWith(challengeToken, Investigator).Start();
        }
    }
}
