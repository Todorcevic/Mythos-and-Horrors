using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RevealChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly IPresenter<RevealChallengeTokenGameAction> _revealTokenPresenter;

        public ChallengeToken ChallengeTokenRevealed { get; private set; }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ChallengeTokenRevealed = _challengeTokensProvider.GetRandomToken();
            await _revealTokenPresenter.PlayAnimationWith(this);
        }
    }
}
