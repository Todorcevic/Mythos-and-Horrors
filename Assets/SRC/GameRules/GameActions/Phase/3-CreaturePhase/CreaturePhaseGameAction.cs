using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CreaturePhaseGameAction : PhaseGameAction, IPhase
    {
        [Inject] private readonly TextsProvider _textsProvider;

        public override Phase MainPhase => Phase.Creature;
        public override string Name => _textsProvider.GameText.CREATURE_PHASE_NAME;
        public override string Description => _textsProvider.GameText.CREATURE_PHASE_DESCRIPTION;

        /*******************************************************************/
        //3.1	Enemy phase begins.
        protected override async Task ExecuteThisPhaseLogic()
        {
            //3.2	Hunter enemies move.
            await _gameActionsProvider.Create<StalkerCreaturesMoveGameAction>().Execute();
            //3.3	Next investigator resolves engaged enemy attacks.
            await _gameActionsProvider.Create<CreatureConfrontAttackGameAction>().Execute();
        }
        //3.4	Enemy phase ends.
    }
}
