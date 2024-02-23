using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class PhaseGameAction : GameAction
    {
        [Inject] private readonly IPresenter<PhaseGameAction> _changePhasePresenter;

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Phase MainPhase { get; }

        public Investigator ActiveInvestigator { get; protected set; }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _changePhasePresenter.PlayAnimationWith(this);
            await ExecuteThisPhaseLogic();
        }

        protected abstract Task ExecuteThisPhaseLogic();
    }
}

