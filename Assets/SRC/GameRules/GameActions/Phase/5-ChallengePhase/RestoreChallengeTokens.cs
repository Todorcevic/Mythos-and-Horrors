using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestoreChallengeTokens : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly IPresenter<RestoreChallengeTokens> _presenter;

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _challengeTokensProvider.RestoreTokens();
            await _presenter.PlayAnimationWith(this);
        }
    }
}
