using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction //2.2	Next investigator's turn begins.
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            ChooseInvestigatorGameAction chooseInvestigatorGA = await _gameActionFactory.Create(new ChooseInvestigatorGameAction(_investigatorsProvider.GetInvestigatorsCanStartTurn));
            ActiveInvestigator = chooseInvestigatorGA.InvestigatorSelected;

            while (ActiveInvestigator.HasTurnsAvailable)
            {
                await _gameActionFactory.Create(new OneInvestigatorTurnGameAction(ActiveInvestigator));
            }
        }       //2.2.2	Investigator's turn ends  
    }
}
