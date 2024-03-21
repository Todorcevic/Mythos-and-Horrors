using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsPhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public override Phase MainPhase => Phase.Investigator;
        public override string Name => _textsProvider.GameText.INVESTIGATOR_PHASE_NAME;
        public override string Description => _textsProvider.GameText.INVESTIGATOR_PHASE_DESCRIPTION;

        /*******************************************************************/
        public override bool CanBeExecuted => _investigatorsProvider.GetInvestigatorsCanStartTurn.Count() > 0;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            ChooseInvestigatorGameAction chooseInvestigatorGA = await _gameActionsProvider.Create(new ChooseInvestigatorGameAction(_investigatorsProvider.GetInvestigatorsCanStartTurn));
            ActiveInvestigator = chooseInvestigatorGA.InvestigatorSelected;

            await _gameActionsProvider.Create(new PlayInvestigatorGameAction(ActiveInvestigator));
            await _gameActionsProvider.Create(new InvestigatorsPhaseGameAction());
        }
    }
}
