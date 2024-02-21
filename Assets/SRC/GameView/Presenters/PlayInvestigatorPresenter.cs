using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class PlayInvestigatorPresenter : IPresenter
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gamAction)
        {
            if (gamAction is InteractableGameAction) await ReturnInvestigator();
        }

        /*******************************************************************/
        private async Task ReturnInvestigator()
        {
            await _moveCardHandler.ReturnCenterShowCard();
        }
    }
}
