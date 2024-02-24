using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PhaseChangePresenter : IPresenter<PhaseGameAction>
    {
        [Inject] private readonly PhaseComponent _phaseComponent;

        /*******************************************************************/
        async Task IPresenter<PhaseGameAction>.PlayAnimationWith(PhaseGameAction phaseGameAction)
        {
            ShowThisPhase(phaseGameAction);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        private void ShowThisPhase(PhaseGameAction phaseGameAction)
        {
            _phaseComponent.ShowThisPhase(phaseGameAction);
        }
    }
}
