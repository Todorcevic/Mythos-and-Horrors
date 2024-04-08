using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RevealChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly IPresenter<RevealChallengeTokenGameAction> _revealTokenPresenter;

        public ChallengeToken ChallengeTokenRevealed { get; private set; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public RevealChallengeTokenGameAction(ChallengeToken challengeToken)
        {
            ChallengeTokenRevealed = challengeToken;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _challengeTokensProvider.RevealToken(ChallengeTokenRevealed);
            await _revealTokenPresenter.PlayAnimationWith(this);
        }

        public void SetChallengeToken(ChallengeToken challengeToken)
        {
            ChallengeTokenRevealed = challengeToken;
        }
    }
}
