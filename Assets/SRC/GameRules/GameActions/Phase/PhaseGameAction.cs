using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class PhaseGameAction : GameAction
    {
        [Inject] private readonly IPresenter<PhaseGameAction> _changePhasePresenter;

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Phase MainPhase { get; }

        public virtual Investigator ActiveInvestigator { get; protected set; }

        /*******************************************************************/
        protected override sealed async Task ExecuteThisLogic()
        {
            await _changePhasePresenter.PlayAnimationWith(this);
            await ExecuteThisPhaseLogic();
        }

        protected abstract Task ExecuteThisPhaseLogic();

        public override async Task Undo()
        {
            await _changePhasePresenter.PlayAnimationWith(this);
        }
    }
}

