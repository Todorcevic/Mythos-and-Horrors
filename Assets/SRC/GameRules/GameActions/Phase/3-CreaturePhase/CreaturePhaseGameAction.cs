using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CreaturePhaseGameAction : PhaseGameAction, IPhase
    {
        public override Phase MainPhase => Phase.Creature;
        public override Localization PhaseNameLocalization => new("PhaseName_CreaturePhase");
        public override Localization PhaseDescriptionLocalization => new("PhaseDescription_CreaturePhase");

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            await _gameActionsProvider.Create<StalkerCreaturesMoveGameAction>().Execute();
            await _gameActionsProvider.Create<CreatureConfrontAttackGameAction>().Execute();
        }
    }
}
