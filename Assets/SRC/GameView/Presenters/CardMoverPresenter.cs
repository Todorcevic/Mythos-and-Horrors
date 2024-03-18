using Zenject;
using System.Threading.Tasks;
using MythosAndHorrors.GameRules;
using DG.Tweening;
using System.Linq;

namespace MythosAndHorrors.GameView
{
    public class CardMoverPresenter : IPresenter<MoveCardsGameAction>, IPresenter<CreatureAttackGameAction>,
        IPresenter<ChooseInvestigatorGameAction>, IPresenter<InvestigateGameAction>
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<MoveCardsGameAction>.PlayAnimationWith(MoveCardsGameAction moveCardsGameAction)
        {
            if (!moveCardsGameAction.IsSingleMove)
            {
                await _moveCardHandler.MoveCardsToCurrentZones(moveCardsGameAction.Cards, ViewValues.DELAY_TIME_ANIMATION).AsyncWaitForCompletion();
                return;
            }

            if (moveCardsGameAction.Parent.Parent is InitialDrawGameAction)
            {
                await _moveCardHandler.MoveCardWithPreviewWithoutWait(moveCardsGameAction.Card, moveCardsGameAction.Card.CurrentZone).AsyncWaitForCompletion();
                return;
            }
            await _moveCardHandler.MoveCardWithPreviewToZone(moveCardsGameAction.Card, moveCardsGameAction.Card.CurrentZone).AsyncWaitForCompletion();
        }

        async Task IPresenter<CreatureAttackGameAction>.PlayAnimationWith(CreatureAttackGameAction gameAction)
        {
            await _moveCardHandler.MoveCardWithPreviewToZone(gameAction.Creature, gameAction.Investigator.InvestigatorZone).AsyncWaitForCompletion();
            _ = _moveCardHandler.ReturnCard(gameAction.Creature);
        }

        async Task IPresenter<ChooseInvestigatorGameAction>.PlayAnimationWith(ChooseInvestigatorGameAction playEffectGA)
        {
            await _moveCardHandler.ReturnCard(playEffectGA.InvestigatorSelected.AvatarCard).AsyncWaitForCompletion();
        }

        async Task IPresenter<InvestigateGameAction>.PlayAnimationWith(InvestigateGameAction playEffectGA)
        {
            await _moveCardHandler.ReturnCard(playEffectGA.CardPlace).AsyncWaitForCompletion();
        }

        /*******************************************************************/
    }
}
