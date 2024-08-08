using System.Threading.Tasks;
using DG.Tweening;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PhaseChangePresenter : IPresenter<PhaseGameAction>
    {
        [Inject] private readonly PhaseComponent _phaseComponent;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;

        /*******************************************************************/
        async Task IPresenter<PhaseGameAction>.PlayAnimationWith(PhaseGameAction phaseGameAction)
        {
            _phaseComponent.ShowThisPhase(phaseGameAction).SetNotWaitable();
            await _swapInvestigatorHandler.Select(phaseGameAction.ActiveInvestigator).AsyncWaitForCompletion();
        }
    }
}
