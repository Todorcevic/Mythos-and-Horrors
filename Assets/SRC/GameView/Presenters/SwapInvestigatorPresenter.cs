using System.Threading.Tasks;
using DG.Tweening;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class SwapInvestigatorPresenter : IPresenter<IWithInvestigator>
    {
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;

        /*******************************************************************/
        async Task IPresenter<IWithInvestigator>.PlayAnimationWith(IWithInvestigator checkHandSizeGameAction)
        {
            await _swapInvestigatorHandler.Select(checkHandSizeGameAction.Investigator).AsyncWaitForCompletion();
        }
    }
}
