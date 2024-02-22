using Zenject;
using System.Threading.Tasks;
using MythsAndHorrors.GameRules;

namespace MythsAndHorrors.GameView
{
    public class CardMoverPresenter : INewPresenter<MoveCardsGameAction>
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task INewPresenter<MoveCardsGameAction>.PlayAnimationWith(MoveCardsGameAction moveCardsGameAction)
        {
            await MoveCardWith(moveCardsGameAction);
        }

        /*******************************************************************/
        private async Task MoveCardWith(MoveCardsGameAction moveCardsGameAction)
        {
            if (!moveCardsGameAction.IsSingleMove)
            {
                await _moveCardHandler.MoveCardsToZone(moveCardsGameAction.Cards, moveCardsGameAction.ToZone, ViewValues.DELAY_TIME_ANIMATION);
                return;
            }

            if (moveCardsGameAction.Parent.Parent is InitialDrawGameAction)
            {
                await _moveCardHandler.MoveCardWithPreviewWithoutWait(moveCardsGameAction.Card, moveCardsGameAction.ToZone);
                return;
            }
            await _moveCardHandler.MoveCardWithPreviewToZone(moveCardsGameAction.Card, moveCardsGameAction.ToZone);
        }
    }
}
