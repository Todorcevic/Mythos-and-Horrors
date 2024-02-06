using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class CardMoverPresenter : IPresenter
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter.CheckGameAction(GameAction gamAction)
        {
            if (gamAction is MoveCardsGameAction moveCardsGameAction)
                await MoveCardWith(moveCardsGameAction);
        }

        /*******************************************************************/
        private async Task MoveCardWith(MoveCardsGameAction moveCardsGameAction)
        {
            if (!moveCardsGameAction.IsSingleMove)
            {
                await _moveCardHandler.MoveCardsToZone(moveCardsGameAction.Cards, moveCardsGameAction.Zone, ViewValues.DELAY_TIME_ANIMATION);
                return;
            }

            if (moveCardsGameAction.Parent.Parent is InitialDrawGameAction)
            {
                await _moveCardHandler.MoveCardWithPreviewWithoutWait(moveCardsGameAction.Card, moveCardsGameAction.Zone);
                return;
            }
            await _moveCardHandler.MoveCardWithPreviewToZone(moveCardsGameAction.Card, moveCardsGameAction.Zone);
        }
    }
}
