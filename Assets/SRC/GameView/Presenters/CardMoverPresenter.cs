using Zenject;
using System.Threading.Tasks;
using MythosAndHorrors.GameRules;
using DG.Tweening;
using System.Linq;

namespace MythosAndHorrors.GameView
{
    public class CardMoverPresenter
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        public async Task PlayAnimationWith(MoveCardsGameAction moveCardsGameAction)
        {
            if (await CheckSpecialMove(moveCardsGameAction)) return;

            if (!moveCardsGameAction.IsSingleMove || moveCardsGameAction.Cards.Any(card => card.FaceDown.IsActive))
            {
                await _moveCardHandler.MoveCardsToCurrentZones(moveCardsGameAction.Cards, ViewValues.DELAY_TIME_ANIMATION).AsyncWaitForCompletion();
                return;
            }
            await _moveCardHandler.MoveCardWithPreviewToZone(moveCardsGameAction.SingleCard, moveCardsGameAction.SingleCard.CurrentZone).AsyncWaitForCompletion();
        }

        private async Task<bool> CheckSpecialMove(MoveCardsGameAction moveCardsGameAction)
        {
            if (moveCardsGameAction.Parent is InitialDrawGameAction)
            {
                foreach (Card card in moveCardsGameAction.Cards)
                {
                    await _moveCardHandler.MoveCardWithPreviewWithoutWait(card, card.CurrentZone).AsyncWaitForCompletion();
                }
                return true;
            }
            return false;
        }
    }
}
