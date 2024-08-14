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
        public override string Name => _textsProvider.GetLocalizableText("PhaseName_InvestigatorsPhase");
        public override string Description => _textsProvider.GetLocalizableText("PhaseDescription_InvestigatorsPhase");

        /*******************************************************************/
        public override bool CanBeExecuted => _investigatorsProvider.GetInvestigatorsCanStartTurn.Any();

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<ChooseInvestigatorGameAction>().SetWith().Execute();
            await _gameActionsProvider.Create<InvestigatorsPhaseGameAction>().Execute();
        }
    }
}
