using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class RestorePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public override Phase MainPhase => Phase.Restore;
        public override string Name => _textsProvider.GameText.RESTORE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.RESTORE_PHASE_DESCRIPTION;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            //await _gameActionFactory.Create(new InteractableGameAction(Effect.NullEffect));
            await Task.CompletedTask;
        }
    }
}
