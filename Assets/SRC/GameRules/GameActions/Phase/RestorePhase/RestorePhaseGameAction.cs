using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    //4.1	Upkeep phase begins.
    public class RestorePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public override Phase MainPhase => Phase.Restore;
        public override string Name => _textsProvider.GameText.RESTORE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.RESTORE_PHASE_DESCRIPTION;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionProvider.Create(new ResetAllInvestigatorsTurnsGameAction());
            await _gameActionProvider.Create(new ReadyAllCardsGameAction());
            await _gameActionProvider.Create(new AllInvestigatorsDrawCardAndResource());
            await _gameActionProvider.Create(new AllInvestigatorsCheckHandSize());
        }
    }
    //4.6	Upkeep phase ends.
}
