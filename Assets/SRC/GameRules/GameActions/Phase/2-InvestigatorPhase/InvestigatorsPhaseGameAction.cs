using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsPhaseGameAction : PhaseGameAction, IPhase
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public override Phase MainPhase => Phase.Investigator;
        public override bool CanBeExecuted => _investigatorsProvider.GetInvestigatorsCanStartTurn.Any();
        public override Localization PhaseNameLocalization => new("PhaseName_InvestigatorsPhase");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_InvestigatorsPhase");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<ChooseInvestigatorGameAction>().SetWith().Execute();
            await _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Execute();
        }
    }
}
