using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    //3.1	Enemy phase begins.
    public class CreaturePhaseGameAction : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public override Phase MainPhase => Phase.Creature;
        public override string Name => _textsProvider.GameText.CREATURE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.CREATURE_PHASE_DESCRIPTION;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionFactory.Create(new StalkerCreaturesMoveGameAction());


        }
    }
    //3.4	Enemy phase ends.
}
