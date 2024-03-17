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
            ShowThisPhase(phaseGameAction);
            await _swapInvestigatorHandler.Select(phaseGameAction.ActiveInvestigator).AsyncWaitForCompletion();
        }

        /*******************************************************************/
        private void ShowThisPhase(PhaseGameAction phaseGameAction)
        {
            _phaseComponent.ShowThisPhase(phaseGameAction);

        }

        /*******************************************************************/
        Task IPresenter<PhaseGameAction>.UndoAnimationWith(PhaseGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
