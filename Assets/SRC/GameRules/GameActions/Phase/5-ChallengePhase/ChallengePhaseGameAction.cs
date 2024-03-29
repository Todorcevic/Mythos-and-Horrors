using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChallengePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Stat Stat { get; init; }
        public int DifficultValue { get; init; }
        public ChallengeType ChallengeType { get; init; }
        public Effect SuccesEffect { get; init; }
        public Effect FailEffect { get; init; }

        public bool IsSuccessful { get; private set; }

        /*******************************************************************/
        public override Phase MainPhase => Phase.Challenge;
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChallengePhaseGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChallengePhaseGameAction);

        /*******************************************************************/
        public ChallengePhaseGameAction(Stat stat, int difficultValue, Effect succesEffect = null, Effect failEffect = null)
        {
            Stat = stat;
            DifficultValue = difficultValue;
            SuccesEffect = succesEffect;
            FailEffect = failEffect;
            ActiveInvestigator = _investigatorsProvider.GetInvestigatorWithThisStat(Stat);
            ChallengeType = ActiveInvestigator.GetChallengeType(Stat);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(Stat, DifficultValue));
            ChallengeToken revealToken = (await _gameActionsProvider.Create(new RevealChallengeTokenGameAction())).ChallengeTokenRevealed;
            await _gameActionsProvider.Create(new ResolveChallengeTokenGameAction(revealToken));
            IsSuccessful = (await _gameActionsProvider.Create(new ResultChallengeGameAction(Stat, DifficultValue, revealToken, ChallengeType))).IsSuccessful;

            if (IsSuccessful)
            {
                if (SuccesEffect != null) await _gameActionsProvider.Create(new PlayEffectGameAction(SuccesEffect));
            }
            else if (FailEffect != null) await _gameActionsProvider.Create(new PlayEffectGameAction(FailEffect));
        }
    }
}
