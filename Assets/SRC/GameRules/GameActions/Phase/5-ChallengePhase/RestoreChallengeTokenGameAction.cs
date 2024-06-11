using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestoreChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly IPresenter<RestoreChallengeTokenGameAction> _presenter;

        public ChallengeToken ChallengeTokenToRestore { get; init; }

        /*******************************************************************/
        public RestoreChallengeTokenGameAction(ChallengeToken challengeTokenToRestore)
        {
            ChallengeTokenToRestore = challengeTokenToRestore;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _challengeTokensProvider.RestoreSingleToken(ChallengeTokenToRestore);
            await _presenter.PlayAnimationWith(this);
        }
    }
}
