using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorsDrawDangerCard : PhaseGameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly TextsProvider _textsProvider;

        /*******************************************************************/
        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(InvestigatorsDrawDangerCard);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(InvestigatorsDrawDangerCard);
        public override Phase MainPhase => Phase.Scene;
        public override bool CanBeExecuted => ActiveInvestigator != null;

        /*******************************************************************/
        public InvestigatorsDrawDangerCard(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create(new DrawDangerGameAction(ActiveInvestigator));
            await _gameActionsProvider.Create(new InvestigatorsDrawDangerCard(ActiveInvestigator.NextInvestigatorInPlay));
        }
    }
}
