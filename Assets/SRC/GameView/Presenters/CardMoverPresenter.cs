using Zenject;
using System.Threading.Tasks;
using MythosAndHorrors.GameRules;
using DG.Tweening;

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
                await _moveCardHandler.MoveCardsToZone(moveCardsGameAction.Cards, moveCardsGameAction.ToZone, ViewValues.DELAY_TIME_ANIMATION).AsyncWaitForCompletion();
                return;
            }

            if (moveCardsGameAction.Parent.Parent is InitialDrawGameAction)
            {
                await _moveCardHandler.MoveCardWithPreviewWithoutWait(moveCardsGameAction.Card, moveCardsGameAction.ToZone).AsyncWaitForCompletion();
                return;
            }
            await _moveCardHandler.MoveCardWithPreviewToZone(moveCardsGameAction.Card, moveCardsGameAction.ToZone).AsyncWaitForCompletion();
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
        async Task IPresenter<MoveCardsGameAction>.UndoAnimationWith(MoveCardsGameAction moveCardsGameAction)
        {
            if (!moveCardsGameAction.IsSingleMove)
            {
                await _moveCardHandler.MoveCardsToZones(moveCardsGameAction.PreviousZones, ViewValues.DELAY_TIME_ANIMATION).AsyncWaitForCompletion();
                return;
            }
        }

        Task IPresenter<InvestigateGameAction>.UndoAnimationWith(InvestigateGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }

        Task IPresenter<ChooseInvestigatorGameAction>.UndoAnimationWith(ChooseInvestigatorGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }

        Task IPresenter<CreatureAttackGameAction>.UndoAnimationWith(CreatureAttackGameAction gameAction)
        {
            throw new System.NotImplementedException();
        }
    }
}
