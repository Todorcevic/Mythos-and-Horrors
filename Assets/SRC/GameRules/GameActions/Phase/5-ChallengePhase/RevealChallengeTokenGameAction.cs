using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RevealChallengeTokenGameAction : GameAction
    {
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly IPresenter<RevealChallengeTokenGameAction> _revealTokenPresenter;

        public ChallengePhaseGameAction ChallengePhase { get; init; }
        public ChallengeToken ChallengeTokenRevealed { get; private set; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public RevealChallengeTokenGameAction(ChallengePhaseGameAction challengePhase)
        {
            ChallengePhase = challengePhase;
        }

        /*******************************************************************/

        protected override async Task ExecuteThisLogic()
        {
            ChallengeTokenRevealed = _challengeTokensProvider.GetRandomToken();
            ChallengePhase.TokensRevealed.Add(ChallengeTokenRevealed);
            await _revealTokenPresenter.PlayAnimationWith(this);
            await _gameActionsProvider.Create(new ResolveSingleChallengeTokenGameAction(ChallengeTokenRevealed));
        }
    }
}
