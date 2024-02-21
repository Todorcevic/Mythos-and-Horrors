using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction //2.2	Next investigator's turn begins.
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;
        public Investigator ActiveInvestigator { get; }

        /*******************************************************************/
        public PlayInvestigatorGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            while (ActiveInvestigator.InvestigatorCard.Turns.Value > 0)
            {
                await _gameActionFactory.Create(new OneInvestigatorTurnGameAction(ActiveInvestigator));
            }
        }
    }
}
