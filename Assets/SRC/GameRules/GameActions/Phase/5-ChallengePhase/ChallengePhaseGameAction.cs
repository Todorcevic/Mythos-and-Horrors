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
        public int Vs { get; init; }
        public ChallengeType ChallengeType { get; init; }

        /*******************************************************************/
        public override Phase MainPhase => Phase.Challenge;
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(ChallengePhaseGameAction);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(ChallengePhaseGameAction);

        /*******************************************************************/
        public ChallengePhaseGameAction(Stat stat, int vs)
        {
            Stat = stat;
            Vs = vs;
            ActiveInvestigator = _investigatorsProvider.GetInvestigatorWithThisStat(Stat);
            ChallengeType = ActiveInvestigator.GetChallengeType(Stat);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new CommitCardsChallengeGameAction(Stat, Vs));
        }
    }
}
