using Zenject;
using System.Threading.Tasks;
using MythosAndHorrors.GameRules;

namespace MythosAndHorrors.GameView
{
    public class CardMoverPresenter : IPresenter<MoveCardsGameAction>, IPresenter<CreatureAttackGameAction>
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<MoveCardsGameAction>.PlayAnimationWith(MoveCardsGameAction moveCardsGameAction)
        {
            await MoveCardWith(moveCardsGameAction);
        }

        async Task IPresenter<CreatureAttackGameAction>.PlayAnimationWith(CreatureAttackGameAction gameAction)
        {
            await _moveCardHandler.MoveCardWithPreviewToZone(gameAction.Creature, gameAction.Investigator.InvestigatorZone);
            _ = _moveCardHandler.ReturnCard(gameAction.Creature);
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
