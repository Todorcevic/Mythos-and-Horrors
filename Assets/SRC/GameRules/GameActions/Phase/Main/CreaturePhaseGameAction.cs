using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreaturePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public override Phase MainPhase => Phase.Creature;
        public override string Name => _textsProvider.GameText.CREATURE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.CREATURE_PHASE_DESCRIPTION;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            //await _gameActionFactory.Create(new InteractableGameAction(Effect.NullEffect));
            await Task.CompletedTask;
        }
    }
}
