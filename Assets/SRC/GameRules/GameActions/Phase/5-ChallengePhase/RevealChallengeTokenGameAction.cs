using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RevealChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly IPresenter<RevealChallengeTokenGameAction> _revealTokenPresenter;

        public ChallengeToken ChallengeTokenRevealed { get; private set; }
        public Investigator Investigator { get; private set; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public RevealChallengeTokenGameAction SetWith(ChallengeToken challengeToken, Investigator investigator)
        {
            ChallengeTokenRevealed = challengeToken;
            Investigator = investigator;
            return this;
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
