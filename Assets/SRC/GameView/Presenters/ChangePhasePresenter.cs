using System.Threading.Tasks;
using DG.Tweening;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ChangePhasePresenter : INewPresenter<PhaseGameAction>
    {
        [Inject] private readonly PhaseComponent _phaseComponent;

        /*******************************************************************/
        async Task INewPresenter<PhaseGameAction>.PlayAnimationWith(PhaseGameAction phaseGameAction)
        {
            await ShowThisPhase(phaseGameAction);
        }

        /*******************************************************************/
        private async Task ShowThisPhase(PhaseGameAction phaseGameAction)
        {
            await _phaseComponent.ShowThisPhase(phaseGameAction).AsyncWaitForCompletion();
        }
    }
}
