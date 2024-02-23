using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : PhaseGameAction //2.2.1	Investigator takes an action, if able.
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly TextsProvider _textsProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        public OneInvestigatorTurnGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionFactory.Create(new InteractableGameAction(isMandatary: false));

            await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.InvestigatorCard.Turns, 1)); //Probablably must be in the concrete action
        }
    }
}
