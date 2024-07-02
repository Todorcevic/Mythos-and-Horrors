using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsPhaseGameAction : PhaseGameAction, IPhase
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public override Phase MainPhase => Phase.Investigator;
        public override string Name => _textsProvider.GameText.INVESTIGATOR_PHASE_NAME;
        public override string Description => _textsProvider.GameText.INVESTIGATOR_PHASE_DESCRIPTION;

        /*******************************************************************/
        public override bool CanBeExecuted => _investigatorsProvider.GetInvestigatorsCanStartTurn.Any();

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<ChooseInvestigatorGameAction>().SetWith().Start();
            await _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Start();
        }
    }
}
