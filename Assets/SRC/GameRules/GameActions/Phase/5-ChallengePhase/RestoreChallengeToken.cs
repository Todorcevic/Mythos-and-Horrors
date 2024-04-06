using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestoreChallengeToken : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly IPresenter<RestoreChallengeToken> _presenter;

        public ChallengeToken ChallengeTokenToRestore { get; init; }

        /*******************************************************************/
        public RestoreChallengeToken(ChallengeToken challengeTokenToRestore)
        {
            ChallengeTokenToRestore = challengeTokenToRestore;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _challengeTokensProvider.RestoreSingleToken(ChallengeTokenToRestore.TokenType);
            await _presenter.PlayAnimationWith(this);
        }
    }
}
