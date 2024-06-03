using DG.Tweening;
using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InteractablePresenter : IInteractablePresenter
    {
        private bool mustShowInCenter;
        [Inject] private readonly BasicShowSelectorComponent _showSelectorComponent;
        [Inject] private readonly MultiEffectHandler _multiEffectHandler;
        [Inject] private readonly ActivatePlayablesHandler _showCardHandler;
        [Inject] private readonly ClickHandler<IPlayable> _clickHandler;
        [Inject] private readonly CardViewsManager _cardViewManager;
        [Inject] private readonly SwapInvestigatorHandler _swapInvestigatorHandler;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ZoneViewsManager _zoneViewsManager;
        [Inject] private readonly MoveCardHandler _moveCardHandler;
        [Inject] private readonly MainButtonComponent _mainButtonComponent;

        /*******************************************************************/
        async Task<BaseEffect> IInteractablePresenter.SelectWith(GameAction gamAction)
        {
            if (gamAction is not InteractableGameAction interactableGameAction) return default;
            _mainButtonComponent.MainButtonEffect = interactableGameAction.MainButtonEffect;
            await _swapInvestigatorHandler.Select(interactableGameAction.ActiveInvestigator).AsyncWaitForCompletion();
            mustShowInCenter = interactableGameAction.MustShowInCenter;
            return await Initial(interactableGameAction, interactableGameAction.Description);
        }

        /*******************************************************************/
        public async Task<BaseEffect> Initial(InteractableGameAction interactableGameAction, string title)
        {
            await DotweenExtension.WaitForMoveToZoneComplete();

            if (interactableGameAction.IsMultiEffect && mustShowInCenter)
            {
                return await InteractWithMultiEfefct(interactableGameAction, _cardViewManager.GetCardView(interactableGameAction.UniqueCard));
            }
            else if (mustShowInCenter)
            {
                List<CardView> cardsToShow = _cardViewsManager.GetAllCanPlay();
                await _showSelectorComponent.ShowCards(cardsToShow, title);
                return await Interact(interactableGameAction);
            }
            else
            {
                await CenterShowDown();
                return await Interact(interactableGameAction);
            }
        }

        private async Task<BaseEffect> Interact(InteractableGameAction interactableGameAction)
        {
            _showCardHandler.ActiavatePlayables();

            IPlayable playableChoose = await _clickHandler.WaitingClick();
            await _showCardHandler.DeactivatePlayables();
            if (playableChoose is ShowCardsInCenterButton)
            {
                mustShowInCenter = !mustShowInCenter;
                return await Initial(interactableGameAction, interactableGameAction.Description);
            }
            await CenterShowDown();

            if (playableChoose.IsMultiEffect)
            {
                return await InteractWithMultiEfefct(interactableGameAction, (CardView)playableChoose);
            }

            return playableChoose.EffectsSelected.FirstOrDefault();
        }

        private async Task<BaseEffect> InteractWithMultiEfefct(InteractableGameAction interactableGameAction, CardView multiEffectCardView)
        {
            mustShowInCenter = interactableGameAction.MustShowInCenter;
            return await _multiEffectHandler.ShowMultiEffects(multiEffectCardView, interactableGameAction.Description)
                ?? await Initial(interactableGameAction, interactableGameAction.Description);
        }

        private async Task CenterShowDown()
        {
            if (_showSelectorComponent.IsShowing)
            {
                List<CardView> cardsToShow = _cardViewsManager.GetAllCanPlay();
                Tween returnSequence = _moveCardHandler.MoveCardsToCurrentZones(cardsToShow.Select(cardView => cardView.Card), ease: Ease.OutSine);
                await _showSelectorComponent.ShowDown(returnSequence, withActivation: false);
            }
        }
    }
}
