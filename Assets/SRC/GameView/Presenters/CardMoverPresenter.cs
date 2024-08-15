using Zenject;
using System.Threading.Tasks;
using MythosAndHorrors.GameRules;
using DG.Tweening;

namespace MythosAndHorrors.GameView
{
    public class CardMoverPresenter : IPresenter<MoveCardsGameAction>, IPresenter<CreatureAttackGameAction>
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<MoveCardsGameAction>.PlayAnimationWith(MoveCardsGameAction moveCardsGameAction)
        {
            if (await CheckSpecialMove(moveCardsGameAction)) return;

            if (!moveCardsGameAction.IsSingleMove)
            {
                await _moveCardHandler.MoveCardsToCurrentZones(moveCardsGameAction.Cards, ViewValues.DELAY_TIME_ANIMATION).AsyncWaitForCompletion();
                return;
            }
            await _moveCardHandler.MoveCardWithPreviewToZone(moveCardsGameAction.SingleCard, moveCardsGameAction.SingleCard.CurrentZone).AsyncWaitForCompletion();
        }

        async Task IPresenter<CreatureAttackGameAction>.PlayAnimationWith(CreatureAttackGameAction gameAction)
        {
            await _moveCardHandler.MoveCardWithPreviewToZone(gameAction.Creature, gameAction.Investigator.InvestigatorZone).AsyncWaitForCompletion();
            _ = _moveCardHandler.ReturnCard(gameAction.Creature);
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
