using System.Threading.Tasks;
using DG.Tweening;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class PhasePresenter : IPresenter
    {
        [Inject] private readonly PhaseComponent _phaseComponent;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gameAction)
        {
            if (gameAction is PhaseGameAction phaseGameAction) await ShowThisPhase(phaseGameAction);
        }

        /*******************************************************************/
        private async Task ShowThisPhase(PhaseGameAction phaseGameAction)
        {
            await _phaseComponent.ShowThisPhase(phaseGameAction).AsyncWaitForCompletion();
        }
    }
}
