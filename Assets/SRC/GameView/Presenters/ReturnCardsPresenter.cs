using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class ReturnCardsPresenter : INewPresenter<InteractableGameAction>
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task INewPresenter<InteractableGameAction>.PlayAnimationWith(InteractableGameAction gameAction)
        {
            await ReturnInvestigator();
        }

        /*******************************************************************/
        private async Task ReturnInvestigator()
        {
            await _moveCardHandler.ReturnCenterShowCard();
        }
    }
}
