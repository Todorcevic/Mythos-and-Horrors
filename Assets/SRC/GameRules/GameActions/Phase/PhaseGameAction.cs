using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public abstract class PhaseGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Phase MainPhase { get; }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _viewLayerProvider.PlayAnimationWith(this);
            await ExecuteThisPhaseLogic();
        }

        protected abstract Task ExecuteThisPhaseLogic();
    }
}

