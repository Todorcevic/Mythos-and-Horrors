using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayInvestigatorGameAction : PhaseGameAction //2.2	Next investigator's turn begins.
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            ChooseInvestigatorGameAction chooseInvestigatorGA = await _gameActionFactory.Create(new ChooseInvestigatorGameAction());

            while (chooseInvestigatorGA.ActiveInvestigator.HasTurnsAvailable)
            {
                await _gameActionFactory.Create(new OneInvestigatorTurnGameAction(chooseInvestigatorGA.ActiveInvestigator));
            }
        }
    }
}
