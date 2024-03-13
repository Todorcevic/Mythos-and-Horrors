using System.Threading.Tasks;
using DG.Tweening;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class SwapInvestigatorPresenter : IPresenter<CheckHandSizeGameAction>
    {
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;

        /*******************************************************************/
        async Task IPresenter<CheckHandSizeGameAction>.PlayAnimationWith(CheckHandSizeGameAction checkHandSizeGameAction)
        {
            await _swapInvestigatorHandler.Select(checkHandSizeGameAction.Investigator).AsyncWaitForCompletion();
        }
    }
}
