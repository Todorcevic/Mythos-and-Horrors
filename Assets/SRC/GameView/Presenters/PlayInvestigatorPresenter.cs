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
            if (gamAction is PlayInvestigatorGameAction playInvestigatorGameAction)
                await ReturnInvestigator(playInvestigatorGameAction);
        }

        /*******************************************************************/
        private async Task ReturnInvestigator(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            await _moveCardHandler.MoveCardWithPreviewToZone(playInvestigatorGameAction.ActiveInvestigator.AvatarCard, playInvestigatorGameAction.ActiveInvestigator.AvatarCard.CurrentZone);
        }
    }
}
