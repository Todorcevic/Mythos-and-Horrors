using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public abstract class PhaseGameAction : GameAction
    {
        public abstract Localization PhaseNameLocalization { get; }
        public abstract Localization PhaseDescriptionLocalization { get; }
        public abstract Phase MainPhase { get; }
        public virtual Investigator ActiveInvestigator { get; protected set; }

        /*******************************************************************/
        protected override sealed async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<VoidGameAction>().Execute(); //To refresh buffs
            await ExecuteThisPhaseLogic();
        }

        protected abstract Task ExecuteThisPhaseLogic();
    }
}

